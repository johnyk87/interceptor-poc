namespace InterceptorPOC.Tests.Targets
{
    using InterceptorPOC.Tests.Helpers;
    using InterceptorPOC.Tests.Interceptors;

    public class ClassWithoutParameterlessConstructor
    {
        private readonly Tracker tracker;

        public ClassWithoutParameterlessConstructor(Tracker tracker)
        {
            this.tracker = tracker;
        }

        [TestInterceptor]
        // WARN: Public methods must be virtual so that they can be intercepted by class proxies
        public virtual void MethodWithTestInterceptor()
        {
            this.SignalTracker();
        }

        private void SignalTracker()
        {
            this.tracker.TargetCalled(this);
        }
    }
}
