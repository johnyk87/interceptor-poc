namespace InterceptorPOC.Tests.Targets
{
    using InterceptorPOC.Tests.Helpers;

    public class ClassWithoutAttributes
    {
        private readonly Tracker tracker;

        public ClassWithoutAttributes(Tracker tracker)
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
