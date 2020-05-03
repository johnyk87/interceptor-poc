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
            var id = Guid.NewGuid();

            try
            {
                this.BeforeInvocation(id, invocation);

                invocation.Proceed();

                if (IsAsync(invocation))
                {
                    isSynchronous = false;

                    ((Task)invocation.ReturnValue).ContinueWith(async task =>
                    {
                        try
                        {
                            await task;

                            this.AfterInvocation(id, invocation.ReturnValue);
                        }
                        catch (Exception ex)
                        {
                            this.OnError(id, ex);
                        }
                        finally
                        {
                            this.OnExit(id);
                        }
                    });
                }
                else
                {
                    this.AfterInvocation(id, invocation.ReturnValue);
                }
            }
            catch (Exception ex)
            {
                this.OnError(id, ex);

                throw;
            }
            finally
            {
                if (isSynchronous)
                {
                    this.OnExit(id);
                }
            }
        }

        protected virtual void BeforeInvocation(Guid id, IInvocation invocation)
        {
        }

        protected virtual void AfterInvocation(Guid id, object returnValue)
        {
        }

        protected virtual void OnError(Guid id, Exception exception)
        {
        }

        protected virtual void OnExit(Guid id)
        {
        }

        private static bool IsAsync(IInvocation invocation)
        {
            return typeof(Task).IsAssignableFrom(invocation.Method.ReturnType);
        }
    }
}
