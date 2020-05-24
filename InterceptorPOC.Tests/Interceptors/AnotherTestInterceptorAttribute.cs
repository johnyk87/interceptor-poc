namespace InterceptorPOC.Tests.Interceptors
{
    public class AnotherTestInterceptorAttribute : InterceptorAttribute
    {
        public AnotherTestInterceptorAttribute()
            : base(typeof(AnotherTestInterceptor))
        {
        }
    }
}
