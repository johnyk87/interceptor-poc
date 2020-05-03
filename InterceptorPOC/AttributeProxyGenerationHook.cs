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
            if (ShouldInterceptMember(type, memberInfo))
            {
                throw new InvalidOperationException($"Can't intercept member {type.Name}.{memberInfo.Name}."
                    + " Please make sure the attribute is attached to an interface member or a virtual/overridable class member.");
            }
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return ShouldInterceptMember(type, methodInfo);
        }

        private bool ShouldInterceptMember(Type type, MemberInfo memberInfo)
        {
            return memberInfo.MemberType.HasFlag(MemberTypes.Method)
                && memberInfo.GetInterceptorAttributes().Any();
        }
    }
}
