﻿namespace InterceptorPOC.Interceptors.Some
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Castle.DynamicProxy;
    using InterceptorPOC.Dependencies;

    public class SomeInterceptor : BaseInterceptor
    {
        private readonly SomeDependency tracker;
        private readonly Guid id;

        public SomeInterceptor(SomeDependency tracker)
        {
            this.tracker = tracker;
            this.id = Guid.NewGuid();
        }

        protected override object BeforeInvocation(IInvocation invocation)
        {
            var name = this.GetName(invocation);

            this.tracker.Before(name);

            return name;
        }

        protected override void AfterInvocation(object state)
        {
            this.tracker.After((string)state);
        }

        protected override void OnError(object state, Exception exception)
        {
            this.tracker.Catch((string)state, exception);
        }

        protected override void OnExit(object state)
        {
            this.tracker.Finally((string)state);
        }

        private string GetName(IInvocation invocation)
        {
            var trackAttribute = invocation.Method.GetCustomAttributes<SomeAttribute>().FirstOrDefault();
            return (trackAttribute?.Name ?? $"{invocation.TargetType.Name}.{invocation.Method.Name}") + " " + this.id;
        }
    }
}
