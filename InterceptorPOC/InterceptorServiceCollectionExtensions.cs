namespace InterceptorPOC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Castle.DynamicProxy;
    using Microsoft.Extensions.DependencyInjection;

    public static class InterceptorServiceCollectionExtensions
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();

        private static readonly ProxyGenerationOptions AttributeProxyGenerationOptions =
            new ProxyGenerationOptions(new AttributeProxyGenerationHook())
            {
                Selector = new AttributeInterceptorSelector(),
            };

        public static IServiceCollection AddAttributeInterceptors(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var interceptions = new List<(ServiceDescriptor ServiceDescriptor, Type[] InterceptorTypes)>();

            foreach (var serviceDescriptor in services)
            {
                var interceptorTypes = serviceDescriptor
                    .ServiceType
                    .GetInterceptorTypes()
                    .Distinct()
                    .Where(interceptorType => services.IsRegistered(interceptorType))
                    .ToArray();

                if (interceptorTypes.Length > 0)
                {
                    interceptions.Add((serviceDescriptor, interceptorTypes));
                }
            }

            interceptions
                .ForEach(interception =>
                    services.AddInterceptors(interception.ServiceDescriptor, interception.InterceptorTypes));

            return services;
        }

        private static IEnumerable<Type> GetInterceptorTypes(
            this Type serviceType)
        {
            return serviceType
                .GetMembers()
                .SelectMany(member =>
                    member
                        .GetInterceptorAttributes()
                        .Select(interceptorAttribute => interceptorAttribute.InterceptorType));
        }

        internal static IEnumerable<InterceptorAttribute> GetInterceptorAttributes(
            this MemberInfo member)
        {
            return member
                .GetCustomAttributes()
                .Where(attribute => attribute is InterceptorAttribute)
                .Select(attribute => attribute as InterceptorAttribute);
        }

        private static bool IsRegistered(this IServiceCollection services, Type serviceType)
        {
            return services.Any(serviceDescriptor => serviceDescriptor.ServiceType == serviceType);
        }

        private static IServiceCollection AddInterceptors(
            this IServiceCollection services,
            ServiceDescriptor serviceDescriptor,
            params Type[] interceptorTypes)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (serviceDescriptor == null)
            {
                throw new ArgumentNullException(nameof(serviceDescriptor));
            }

            services.Remove(serviceDescriptor);

            object proxyFactory(IServiceProvider serviceProvider) =>
                CreateProxyWithTarget(
                    serviceDescriptor.ServiceType,
                    serviceDescriptor.GetImplementationInstance(serviceProvider),
                    AttributeProxyGenerationOptions,
                    interceptorTypes.ResolveInstances<IInterceptor>(serviceProvider));

            services.Add(new ServiceDescriptor(serviceDescriptor.ServiceType, proxyFactory, serviceDescriptor.Lifetime));

            return services;
        }

        private static object CreateProxyWithTarget(
            Type serviceType,
            object target,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
        {
            if (serviceType.IsInterface)
            {
                return ProxyGenerator.CreateInterfaceProxyWithTarget(
                    serviceType,
                    target,
                    proxyGenerationOptions,
                    interceptors);
            }
            else
            {
                return ProxyGenerator.CreateClassProxyWithTarget(
                    serviceType,
                    target,
                    proxyGenerationOptions,
                    interceptors);
            }
        }

        private static object GetImplementationInstance(
            this ServiceDescriptor serviceDescriptor,
            IServiceProvider serviceProvider)
        {
            if (serviceDescriptor == null)
            {
                throw new ArgumentNullException(nameof(serviceDescriptor));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (serviceDescriptor.ImplementationInstance != null)
            {
                return serviceDescriptor.ImplementationInstance;
            }
            else if (serviceDescriptor.ImplementationFactory != null)
            {
                return serviceDescriptor.ImplementationFactory(serviceProvider);
            }
            else if (serviceDescriptor.ImplementationType != null)
            {
                return ActivatorUtilities.CreateInstance(serviceProvider, serviceDescriptor.ImplementationType);
            }
            else
            {
                throw new NotImplementedException("Unknown service descriptor type.");
            }
        }

        private static T[] ResolveInstances<T>(this Type[] types, IServiceProvider serviceProvider)
        {
            return types
                .Select(type => (T)serviceProvider.GetRequiredService(type))
                .ToArray();
        }
    }
}
