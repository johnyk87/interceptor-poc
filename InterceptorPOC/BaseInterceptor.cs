namespace InterceptorPOC
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Castle.DynamicProxy;

    public abstract class BaseInterceptor : IInterceptor
    {
        private static readonly MethodInfo VoidHandler = typeof(BaseInterceptor)
            .GetMethod(nameof(InterceptVoid), BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo ResultHandler = typeof(BaseInterceptor)
            .GetMethod(nameof(InterceptWithResult), BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo VoidTaskHandler = typeof(BaseInterceptor)
            .GetMethod(nameof(InterceptVoidAsync), BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo ResultTaskHandler = typeof(BaseInterceptor)
            .GetMethod(nameof(InterceptWithResultAsync), BindingFlags.Instance | BindingFlags.NonPublic);

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = this.InvokeHandlerMethod(invocation);
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

        private static bool IsTask(Type returnType)
        {
            return typeof(Task).IsAssignableFrom(returnType);
        }

        private object InvokeHandlerMethod(IInvocation invocation)
        {
            MethodInfo handler;

            if (IsTask(invocation.Method.ReturnType))
            {
                handler = invocation.Method.ReturnType.GenericTypeArguments.Length > 0
                    ? ResultTaskHandler.MakeGenericMethod(invocation.Method.ReturnType.GenericTypeArguments)
                    : VoidTaskHandler;
            }
            else
            {
                handler = invocation.Method.ReturnType != typeof(void)
                    ? ResultHandler.MakeGenericMethod(invocation.Method.ReturnType)
                    : VoidHandler;
            }

            return handler.Invoke(this, new object[] { invocation});
        }

        private void InterceptVoid(IInvocation invocation)
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

        private T InterceptWithResult<T>(IInvocation invocation)
        {
            object state = null;

            try
            {
                state = this.BeforeInvocation(invocation);

                invocation.Proceed();

                var result = (T)invocation.ReturnValue;

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

        private async Task InterceptVoidAsync(IInvocation invocation)
        {
            object state = null;

            try
            {
                state = this.BeforeInvocation(invocation);

                invocation.Proceed();

                await (Task)invocation.ReturnValue;

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

        private async Task<T> InterceptWithResultAsync<T>(IInvocation invocation)
        {
            object state = null;

            try
            {
                state = this.BeforeInvocation(invocation);

                invocation.Proceed();

                var result = await (Task<T>)invocation.ReturnValue;

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
