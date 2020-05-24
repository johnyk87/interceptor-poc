namespace InterceptorPOC.Tests.Helpers
{
    using System.Collections.Concurrent;

    public class Tracker
    {
        public readonly ConcurrentQueue<object> TargetsCalled = new ConcurrentQueue<object>();
        public readonly ConcurrentQueue<object> InterceptorsCalled = new ConcurrentQueue<object>();

        public int TargetCalls => this.TargetsCalled.Count;

        public int InterceptorCalls => this.InterceptorsCalled.Count;

        public void TargetCalled(object target)
        {
            this.TargetsCalled.Enqueue(target);
        }

        public void InterceptorCalled(object interceptor)
        {
            this.InterceptorsCalled.Enqueue(interceptor);
        }
    }
}
