namespace InterceptorPOC.Interceptors.Some
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Castle.DynamicProxy;
    using InterceptorPOC.Dependencies;

    public class SomeInterceptor : BaseInterceptor
    {
        private readonly IDictionary<Guid, string> Names = new Dictionary<Guid, string>();
        private readonly SomeDependency tracker;
        private readonly Guid id;

        public SomeInterceptor(SomeDependency tracker)
        {
            this.tracker = tracker;
            this.id = Guid.NewGuid();
        }

        protected override void BeforeInvocation(Guid id, IInvocation invocation)
        {
            var name = this.GetName(invocation);

            this.Names[id] = name;

            this.tracker.Before(name);
        }

        protected override void AfterInvocation(Guid id, object returnValue)
        {
            this.tracker.After(this.Names[id]);
        }

        protected override void OnError(Guid id, Exception exception)
        {
            this.tracker.Catch(this.Names[id], exception);
        }

        protected override void OnExit(Guid id)
        {
            this.tracker.Finally(this.Names[id]);
        }

        private string GetName(IInvocation invocation)
        {
            var trackAttribute = invocation.Method.GetCustomAttributes<SomeAttribute>().FirstOrDefault();
            return (trackAttribute?.Name ?? $"{invocation.TargetType.Name}.{invocation.Method.Name}") + " " + this.id;
        }
    }
}
