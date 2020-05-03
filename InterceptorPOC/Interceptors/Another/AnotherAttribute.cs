namespace InterceptorPOC.Interceptors.Another
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AnotherAttribute : InterceptorAttribute
    {
        public AnotherAttribute(string name)
            : base(typeof(AnotherInterceptor))
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
