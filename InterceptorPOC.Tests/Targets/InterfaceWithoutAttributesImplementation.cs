namespace InterceptorPOC.Tests.Targets
{
    using InterceptorPOC.Tests.Helpers;

    public class InterfaceWithoutAttributesImplementation : IInterfaceWithoutAttributes
    {
        private readonly Tracker tracker;

        public InterfaceWithoutAttributesImplementation(Tracker tracker)
        {
            this.tracker = tracker;
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
