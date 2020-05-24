namespace InterceptorPOC.Tests.Targets
{
    using InterceptorPOC.Tests.Interceptors;

    public interface IInterfaceWithAttributes
    {
        [TestInterceptor]
        void MethodWithTestInterceptor();

        [TestInterceptor]
        [AnotherTestInterceptor]
        void MethodWithMultipleTestInterceptors();

        [AnotherTestInterceptor]
        [TestInterceptor]
        void MethodWithMultipleTestInterceptorsAndInvertedOrder();

        void MethodWithoutAttribute();
    }
}
