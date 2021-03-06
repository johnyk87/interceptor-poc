﻿namespace InterceptorPOC
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
            .GetMethod(nameof(InterceptVoidTaskAsync), BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo ResultTaskHandler = typeof(BaseInterceptor)
            .GetMethod(nameof(InterceptTaskWithResultAsync), BindingFlags.Instance | BindingFlags.NonPublic);

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = this.InvokeHandlerMethod(invocation);
        }

        protected virtual object BeforeInvocation(IInvocation invocation)
        {
            return default;
        }

        protected virtual void AfterInvocation(object state)
        {
        }

        protected virtual bool OnError(object state, Exception exception)
        {
            return false;
        }

        protected virtual void OnExit(object state)
        {
        }

        protected virtual object BeforeInvocation<TResult>(IInvocation invocation)
        {
            return this.BeforeInvocation(invocation);
        }

        protected virtual TResult AfterInvocation<TResult>(object state, TResult result)
        {
            this.AfterInvocation(state);

            return result;
        }

        protected virtual (bool, TResult) OnError<TResult>(object state, Exception exception)
        {
            var isHandled = this.OnError(state, exception);

            return (isHandled, default);
        }

        protected virtual void OnExit<TResult>(object state)
        {
            this.OnExit(state);
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
            object state = null;

            try
            {
                state = this.BeforeInvocation(invocation);

                invocation.Proceed();

                this.AfterInvocation(state);
            }
            catch (Exception ex)
            {
                var isHandled = this.OnError(state, ex);

                if (!isHandled)
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
                state = this.BeforeInvocation<T>(invocation);

                invocation.Proceed();

                var result = (T)invocation.ReturnValue;

                return this.AfterInvocation(state, result);
            }
            catch (Exception ex)
            {
                var (isHandled, result) = this.OnError<T>(state, ex);

                if (!isHandled)
                {
                    throw;
                }

                return result;
            }
            finally
            {
                this.OnExit<T>(state);
            }
        }

        private async Task InterceptVoidTaskAsync(IInvocation invocation)
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
                var isHandled = this.OnError(state, ex);

                if (!isHandled)
                {
                    throw;
                }
            }
            finally
            {
                this.OnExit(state);
            }
        }

        private async Task<T> InterceptTaskWithResultAsync<T>(IInvocation invocation)
        {
            object state = null;

            try
            {
                state = this.BeforeInvocation<T>(invocation);

                invocation.Proceed();

                var result = await (Task<T>)invocation.ReturnValue;

                return this.AfterInvocation(state, result);
            }
            catch (Exception ex)
            {
                var (isHandled, result) = this.OnError<T>(state, ex);

                if (!isHandled)
                {
                    throw;
                }

                return result;
            }
            finally
            {
                this.OnExit<T>(state);
            }
        }
    }
}
