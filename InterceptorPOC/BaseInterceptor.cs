namespace InterceptorPOC
{
    using System;
    using System.Threading.Tasks;
    using Castle.DynamicProxy;

    public abstract class BaseInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var isSynchronous = true;
            object state = null;

            try
            {
                state = this.BeforeInvocation(invocation);

                invocation.Proceed();

                if (IsAsync(invocation))
                {
                    isSynchronous = false;

                    ((Task)invocation.ReturnValue).ContinueWith(async task =>
                    {
                        try
                        {
                            await task;

                            this.AfterInvocation(state);
                        }
                        catch (Exception ex)
                        {
                            this.OnError(state, ex);
                        }
                        finally
                        {
                            this.OnExit(state);
                        }
                    });
                }
                else
                {
                    this.AfterInvocation(state);
                }
            }
            catch (Exception ex)
            {
                this.OnError(state, ex);

                throw;
            }
            finally
            {
                if (isSynchronous)
                {
                    this.OnExit(state);
                }
            }
        }

        protected virtual object BeforeInvocation(IInvocation invocation)
        {
            return invocation;
        }

        protected virtual void AfterInvocation(object state)
        {
        }

        protected virtual void OnError(object state, Exception exception)
        {
        }

        protected virtual void OnExit(object state)
        {
        }

        private static bool IsAsync(IInvocation invocation)
        {
            return typeof(Task).IsAssignableFrom(invocation.Method.ReturnType);
        }
    }
}
