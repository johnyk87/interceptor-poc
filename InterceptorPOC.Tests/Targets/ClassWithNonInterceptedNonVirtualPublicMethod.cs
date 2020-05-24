namespace InterceptorPOC.Tests.Targets
{
    using InterceptorPOC.Tests.Interceptors;

    public class ClassWithNonInterceptedNonVirtualPublicMethod
    {
        [TestInterceptor]
        public virtual void MethodWithTestInterceptor()
        {
        }

        public void NonVirtualMethodWithoutAttribute()
        {
        }
    }
}
