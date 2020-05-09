namespace InterceptorPOC.Interceptors.Async
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Castle.DynamicProxy;
    using InterceptorPOC.Dependencies;

    public class AsyncInterceptor : BaseAsyncInterceptor
    {
        private readonly SomeDependency tracker;
        private readonly Guid id;

        public AsyncInterceptor(SomeDependency tracker)
        {
            this.tracker = tracker;
            this.id = Guid.NewGuid();
        }

        protected override Task<object> BeforeInvocationAsync(IInvocation invocation)
        {
            var name = this.GetName(invocation);

            this.tracker.Before(name);

            return Task.FromResult<object>(name);
        }

        protected override Task AfterInvocationAsync(object state)
        {
            this.tracker.After((string)state);

            return Task.CompletedTask;
        }

        protected override Task<bool> OnErrorAsync(object state, Exception exception)
        {
            this.tracker.Catch((string)state, exception);

            return Task.FromResult(false);
        }

        protected override Task OnExitAsync(object state)
        {
            this.tracker.Finally((string)state);

            return Task.CompletedTask;
        }

        private string GetName(IInvocation invocation)
        {
            var trackAttribute = invocation.Method.GetCustomAttributes<AsyncAttribute>().FirstOrDefault();
            return (trackAttribute?.Name ?? $"{invocation.TargetType.Name}.{invocation.Method.Name}") + " " + this.id;
        }
    }
}
