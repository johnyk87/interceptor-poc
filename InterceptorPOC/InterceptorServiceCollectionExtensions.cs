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

        public static IServiceCollection AddAttributeInterception(this IServiceCollection services)
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
                    .Where(services.IsRegistered)
                    .ToArray();

                if (interceptorTypes.Length > 0)
                {
                    interceptions.Add((serviceDescriptor, interceptorTypes));
                }
            }

            interceptions
                .ForEach(interception =>
                    services.ReplaceWithProxy(interception.ServiceDescriptor, interception.InterceptorTypes));

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
                .OfType<InterceptorAttribute>();
        }

        private static bool IsRegistered(this IServiceCollection services, Type serviceType)
        {
            return services.Any(serviceDescriptor => serviceDescriptor.ServiceType == serviceType);
        }

        private static void ReplaceWithProxy(
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

            object ProxyFactory(IServiceProvider serviceProvider) =>
                CreateProxyWithTarget(
                    serviceDescriptor.ServiceType,
                    serviceDescriptor.GetImplementationInstance(serviceProvider),
                    AttributeProxyGenerationOptions,
                    serviceProvider.GetRequiredServices<IInterceptor>(interceptorTypes).ToArray());

            services.Add(new ServiceDescriptor(serviceDescriptor.ServiceType, ProxyFactory, serviceDescriptor.Lifetime));
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

            if (serviceDescriptor.ImplementationFactory != null)
            {
                return serviceDescriptor.ImplementationFactory(serviceProvider);
            }

            if (serviceDescriptor.ImplementationType != null)
            {
                return ActivatorUtilities.CreateInstance(serviceProvider, serviceDescriptor.ImplementationType);
            }

            throw new NotSupportedException("Unknown service descriptor type.");
        }

        private static IEnumerable<T> GetRequiredServices<T>(
            this IServiceProvider serviceProvider,
            IEnumerable<Type> types)
        {
            return types.Select(type => (T)serviceProvider.GetRequiredService(type));
        }
    }
}
