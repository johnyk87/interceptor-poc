namespace InterceptorPOC.Interceptors.Sync
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SyncAttribute : InterceptorAttribute
    {
        public SyncAttribute(string name)
            : base(typeof(SyncInterceptor))
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
