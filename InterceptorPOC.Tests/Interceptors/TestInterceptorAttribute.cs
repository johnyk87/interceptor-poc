namespace InterceptorPOC.Tests.Interceptors
{
    public class TestInterceptorAttribute : InterceptorAttribute
    {
        public TestInterceptorAttribute()
            : base(typeof(TestInterceptor))
        {
        }
    }
}
