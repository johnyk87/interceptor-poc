namespace InterceptorPOC.Tests.Targets
{
    using InterceptorPOC.Tests.Helpers;
    using InterceptorPOC.Tests.Interceptors;

    public class ClassWithInterceptedProtectedMethod
    {
        private readonly Tracker tracker;

        // WARN: A parameterless constructor is required for class proxy generation
        protected ClassWithInterceptedProtectedMethod()
        {
        }

        public ClassWithInterceptedProtectedMethod(Tracker tracker)
        {
            this.tracker = tracker;
        }

        // WARN: Public methods must be virtual so that they can be intercepted by class proxies
        public virtual void MethodWithoutAttribute()
        {
            this.ProtectedMethodWithTestInterceptor();
        }

        [TestInterceptor]
        // WARN: Protected methods with attribute won't be intercepted and no error will be thrown
        protected virtual void ProtectedMethodWithTestInterceptor()
        {
            this.tracker.TargetCalled(this);
        }
    }
}
