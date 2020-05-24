namespace InterceptorPOC.Tests.Targets
{
    using InterceptorPOC.Tests.Helpers;
    using InterceptorPOC.Tests.Interceptors;

    public class ClassWithInterceptedPrivateMethod
    {
        private readonly Tracker tracker;

        // WARN: A parameterless constructor is required for class proxy generation
        protected ClassWithInterceptedPrivateMethod()
        {
        }

        public ClassWithInterceptedPrivateMethod(Tracker tracker)
        {
            this.tracker = tracker;
        }

        // WARN: Public methods must be virtual so that they can be intercepted by class proxies
        public virtual void MethodWithoutAttribute()
        {
            this.PrivateMethodWithTestInterceptor();
        }

        [TestInterceptor]
        // WARN: Private methods with attribute won't be intercepted and no error will be thrown
        private void PrivateMethodWithTestInterceptor()
        {
            this.tracker.TargetCalled(this);
        }
    }
}
