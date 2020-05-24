namespace InterceptorPOC.Tests.Helpers
{
    using Castle.DynamicProxy;
    using Xunit;

    public static class AssertHelper
    {
        public static void Proxy(object target)
        {
            Assert.True(IsProxy(target));
        }

        public static void NotProxy(object target)
        {
            Assert.False(IsProxy(target));
        }

        private static bool IsProxy(object target)
        {
            return target is IProxyTargetAccessor;
        }
    }
}
