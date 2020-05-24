namespace InterceptorPOC.Tests.Interceptors
{
    using Castle.DynamicProxy;
    using InterceptorPOC.Tests.Helpers;

    public class AnotherTestInterceptor : IInterceptor
    {
        private readonly Tracker tracker;

        public AnotherTestInterceptor(Tracker tracker)
        {
            this.tracker = tracker;
        }

        public void Intercept(IInvocation invocation)
        {
            this.tracker.InterceptorCalled(this);

            invocation.Proceed();
        }
    }
}
