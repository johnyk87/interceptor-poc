namespace InterceptorPOC.Tests.Targets
{
    using InterceptorPOC.Tests.Helpers;
    using InterceptorPOC.Tests.Interceptors;

    public class ClassWithAttributes
    {
        private readonly Tracker tracker;

        // WARN: A parameterless constructor is required for class proxy generation
        protected ClassWithAttributes()
        {
        }

        public ClassWithAttributes(Tracker tracker)
        {
            this.tracker = tracker;
        }

        [TestInterceptor]
        // WARN: Public methods must be virtual so that they can be intercepted by class proxies
        public virtual void MethodWithTestInterceptor()
        {
            this.SignalTracker();
        }

        [TestInterceptor]
        [AnotherTestInterceptor]
        // WARN: Public methods must be virtual so that they can be intercepted by class proxies
        public virtual void MethodWithMultipleTestInterceptors()
        {
            this.SignalTracker();
        }

        [AnotherTestInterceptor]
        [TestInterceptor]
        // WARN: Public methods must be virtual so that they can be intercepted by class proxies
        public virtual void MethodWithMultipleTestInterceptorsAndInvertedOrder()
        {
            this.SignalTracker();
        }

        // WARN: Public methods must be virtual so that they can be intercepted by class proxies
        public virtual void MethodWithoutAttribute()
        {
            this.SignalTracker();
        }

        private void SignalTracker()
        {
            this.tracker.TargetCalled(this);
        }
    }
}
