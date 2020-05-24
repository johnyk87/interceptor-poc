namespace InterceptorPOC.Tests.Targets
{
    using InterceptorPOC.Tests.Interceptors;

    public class ClassWithInterceptedNonVirtualMethod
    {
        [TestInterceptor]
        public void NonVirtualMethodWithTestInterceptor()
        {
        }
    }
}
