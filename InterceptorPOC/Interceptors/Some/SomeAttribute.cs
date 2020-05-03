namespace InterceptorPOC.Interceptors.Some
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SomeAttribute : InterceptorAttribute
    {
        public SomeAttribute(string name)
            : base(typeof(SomeInterceptor))
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
