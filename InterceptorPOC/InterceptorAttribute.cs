namespace InterceptorPOC
{
    using System;
    using Castle.DynamicProxy;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public abstract class InterceptorAttribute : Attribute
    {
        protected InterceptorAttribute(Type interceptorType)
        {
            if (interceptorType == null)
            {
                throw new ArgumentNullException(nameof(interceptorType));
            }

            if (!typeof(IInterceptor).IsAssignableFrom(interceptorType))
            {
                throw new InvalidOperationException($"Type {interceptorType.FullName} is not a valid {nameof(IInterceptor)}.");
            }

            this.InterceptorType = interceptorType;
        }

        public Type InterceptorType { get; }
    }
}
