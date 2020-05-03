namespace InterceptorPOC
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Castle.DynamicProxy;

    internal class AttributeInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return method
                .GetInterceptorAttributes()
                .Select(interceptorAttribute =>
                    interceptors.FirstOrDefault(interceptor => interceptorAttribute.InterceptorType.IsInstanceOfType(interceptor)))
                .Where(interceptor => interceptor != null)
                .ToArray();
        }
    }
}