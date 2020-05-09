namespace InterceptorPOC.Interceptors.Async
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AsyncAttribute : InterceptorAttribute
    {
        public AsyncAttribute(string name)
            : base(typeof(AsyncInterceptor))
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
