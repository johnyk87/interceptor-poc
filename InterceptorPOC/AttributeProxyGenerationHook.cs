namespace InterceptorPOC
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Castle.DynamicProxy;

    internal class AttributeProxyGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            if (!(memberInfo is MethodInfo methodInfo))
            {
                return;
            }

            if (methodInfo.IsPublic)
            {
                throw new InvalidOperationException(
                    $"Can't intercept member {type.FullName}.{memberInfo.Name}."
                    + " Please make sure all public members are virtual/overridable.");
            }

            if (IsInterceptable(methodInfo))
            {
                throw new InvalidOperationException(
                    $"Can't intercept member {type.FullName}.{memberInfo.Name}."
                    + " Please make sure the member is virtual/overridable.");
            }
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodInfo.IsPublic
                || IsInterceptable(methodInfo);
        }

        private static bool IsInterceptable(MethodInfo methodInfo)
        {
            return methodInfo.GetInterceptorAttributes().Any();
        }
    }
}
