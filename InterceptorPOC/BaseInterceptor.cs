namespace InterceptorPOC
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Castle.DynamicProxy;

    public abstract class BaseInterceptor : IInterceptor
    {
        private static readonly MethodInfo VoidTaskHandler = typeof(BaseInterceptor)
            .GetMethod(nameof(HandleVoidTaskReturnValue), BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo GenericTaskHandler = typeof(BaseInterceptor)
            .GetMethod(nameof(HandleGenericTaskReturnValue), BindingFlags.Instance | BindingFlags.NonPublic);

        public void Intercept(IInvocation invocation)
        {
            if (IsAsync(invocation))
            {
                this.InterceptAsyncMethod(invocation);
            }
            else
            {
                this.InterceptSyncMethod(invocation);
            }
        }

        protected virtual object BeforeInvocation(IInvocation invocation)
        {
            return invocation;
        }

        protected virtual void AfterInvocation(object state)
        {
        }

        protected virtual bool OnError(object state, Exception exception, out object newResult)
        {
            newResult = null;

            return false;
        }

        protected virtual void OnExit(object state)
        {
        }

        private static bool IsAsync(IInvocation invocation)
        {
            return typeof(Task).IsAssignableFrom(invocation.Method.ReturnType);
        }

        private void InterceptSyncMethod(IInvocation invocation)
        {
            object state = null;

            try
            {
                state = this.BeforeInvocation(invocation);

                invocation.Proceed();

                this.AfterInvocation(state);
            }
            catch (Exception ex)
            {
                if (!this.OnError(state, ex, out var newResult))
                {
                    throw;
                }

                invocation.ReturnValue = newResult;
            }
            finally
            {
                this.OnExit(state);
            }
        }

        private void InterceptAsyncMethod(IInvocation invocation)
        {
            var isSynchronous = true;
            object state = null;

            try
            {
                state = this.BeforeInvocation(invocation);

                invocation.Proceed();

                invocation.ReturnValue = this.InvokeTaskHandlerMethod(
                    invocation.Method.ReturnType,
                    invocation.ReturnValue,
                    state);

                isSynchronous = false;
            }
            catch (Exception ex)
            {
                if (!this.OnError(state, ex, out var newResult))
                {
                    throw;
                }

                invocation.ReturnValue = newResult;
            }
            finally
            {
                if (isSynchronous)
                {
                    this.OnExit(state);
                }
            }
        }

        private object InvokeTaskHandlerMethod(Type taskType, object task, object state)
        {
            var handler = taskType.GenericTypeArguments.Length > 0
                ? GenericTaskHandler.MakeGenericMethod(taskType.GenericTypeArguments)
                : VoidTaskHandler;

            return handler.Invoke(this, new [] { task, state});
        }

        private async Task HandleVoidTaskReturnValue(Task task, object state)
        {
            try
            {
                await task;

                this.AfterInvocation(state);
            }
            catch (Exception ex)
            {
                if (!this.OnError(state, ex, out _))
                {
                    throw;
                }
            }
            finally
            {
                this.OnExit(state);
            }
        }

        private async Task<T> HandleGenericTaskReturnValue<T>(Task<T> task, object state)
        {
            try
            {
                var result = await task;

                this.AfterInvocation(state);

                return result;
            }
            catch (Exception ex)
            {
                if (!this.OnError(state, ex, out var newResult))
                {
                    throw;
                }

                return (T)newResult;
            }
            finally
            {
                this.OnExit(state);
            }
        }
    }
}
