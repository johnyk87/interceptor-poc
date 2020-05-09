namespace InterceptorPOC
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Castle.DynamicProxy;

    public abstract class BaseAsyncInterceptor : IInterceptor
    {
        private static readonly MethodInfo VoidHandler = typeof(BaseAsyncInterceptor)
            .GetMethod(nameof(InterceptVoid), BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo ResultHandler = typeof(BaseAsyncInterceptor)
            .GetMethod(nameof(InterceptWithResult), BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo VoidTaskHandler = typeof(BaseAsyncInterceptor)
            .GetMethod(nameof(InterceptVoidTaskAsync), BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo ResultTaskHandler = typeof(BaseAsyncInterceptor)
            .GetMethod(nameof(InterceptTaskWithResultAsync), BindingFlags.Instance | BindingFlags.NonPublic);

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = this.InvokeHandlerMethod(invocation);
        }

        protected virtual Task<object> BeforeInvocationAsync(IInvocation invocation)
        {
            return Task.FromResult<object>(default);
        }

        protected virtual Task AfterInvocationAsync(object state)
        {
            return Task.CompletedTask;
        }

        protected virtual Task<bool> OnErrorAsync(object state, Exception exception)
        {
            return Task.FromResult(false);
        }

        protected virtual Task OnExitAsync(object state)
        {
            return Task.CompletedTask;
        }

        protected virtual Task<object> BeforeInvocationAsync<TResult>(IInvocation invocation)
        {
            return this.BeforeInvocationAsync(invocation);
        }

        protected virtual async Task<TResult> AfterInvocationAsync<TResult>(object state, TResult returnValue)
        {
            await this.AfterInvocationAsync(state);

            return returnValue;
        }

        protected virtual async Task<(bool, TResult)> OnErrorAsync<TResult>(object state, Exception exception)
        {
            var isHandled = await this.OnErrorAsync(state, exception);

            return (isHandled, default);
        }

        protected virtual Task OnExitAsync<TResult>(object state)
        {
            return this.OnExitAsync(state);
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

            return handler.Invoke(this, new object[] { invocation });
        }

        private void InterceptVoid(IInvocation invocation)
        {
            this.InterceptVoidAsync(invocation).GetAwaiter().GetResult();
        }

        private T InterceptWithResult<T>(IInvocation invocation)
        {
            return this.InterceptWithResultAsync<T>(invocation).GetAwaiter().GetResult();
        }

        private async Task InterceptVoidAsync(IInvocation invocation)
        {
            object state = null;

            try
            {
                state = await this.BeforeInvocationAsync(invocation);

                invocation.Proceed();

                await this.AfterInvocationAsync(state);
            }
            catch (Exception ex)
            {
                var isHandled = await this.OnErrorAsync(state, ex);

                if (!isHandled)
                {
                    throw;
                }
            }
            finally
            {
                await this.OnExitAsync(state);
            }
        }

        private async Task<T> InterceptWithResultAsync<T>(IInvocation invocation)
        {
            object state = null;

            try
            {
                state = await this.BeforeInvocationAsync<T>(invocation);

                invocation.Proceed();

                var result = (T)invocation.ReturnValue;

                return await this.AfterInvocationAsync(state, result);
            }
            catch (Exception ex)
            {
                var (isHandled, result) = await this.OnErrorAsync<T>(state, ex);

                if (!isHandled)
                {
                    throw;
                }

                return result;
            }
            finally
            {
                await this.OnExitAsync<T>(state);
            }
        }

        private async Task InterceptVoidTaskAsync(IInvocation invocation)
        {
            object state = null;

            try
            {
                state = await this.BeforeInvocationAsync(invocation);

                invocation.Proceed();

                await (Task)invocation.ReturnValue;

                await this.AfterInvocationAsync(state);
            }
            catch (Exception ex)
            {
                var isHandled = await this.OnErrorAsync(state, ex);

                if (!isHandled)
                {
                    throw;
                }
            }
            finally
            {
                await this.OnExitAsync(state);
            }
        }

        private async Task<T> InterceptTaskWithResultAsync<T>(IInvocation invocation)
        {
            object state = null;

            try
            {
                state = await this.BeforeInvocationAsync<T>(invocation);

                invocation.Proceed();

                var result = await (Task<T>)invocation.ReturnValue;

                return await this.AfterInvocationAsync(state, result);
            }
            catch (Exception ex)
            {
                var (isHandled, result) = await this.OnErrorAsync<T>(state, ex);

                if (!isHandled)
                {
                    throw;
                }

                return result;
            }
            finally
            {
                await this.OnExitAsync<T>(state);
            }
        }
    }
}
