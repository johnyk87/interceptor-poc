namespace InterceptorPOC.Tests.Targets
{
    using InterceptorPOC.Tests.Helpers;

    public class InterfaceWithAttributesImplementation : IInterfaceWithAttributes
    {
        private readonly Tracker tracker;

        public InterfaceWithAttributesImplementation(Tracker tracker)
        {
            this.tracker = tracker;
        }

        public void MethodWithTestInterceptor()
        {
            this.SignalTracker();
        }

        public void MethodWithMultipleTestInterceptors()
        {
            this.SignalTracker();
        }

        public void MethodWithMultipleTestInterceptorsAndInvertedOrder()
        {
            this.SignalTracker();
        }

        public void MethodWithoutAttribute()
        {
            this.SignalTracker();
        }

        private void SignalTracker()
        {
            this.tracker.TargetCalled(this);
        }
    }
}
