namespace InterceptorPOC.Tests.Interceptors
{
    using Castle.DynamicProxy;
    using InterceptorPOC.Tests.Helpers;

    public class TestInterceptor : IInterceptor
    {
        private readonly Tracker tracker;

        public TestInterceptor(Tracker tracker)
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
