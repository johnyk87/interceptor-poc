namespace InterceptorPOC.Tests
{
    using InterceptorPOC.Tests.Helpers;
    using InterceptorPOC.Tests.Interceptors;
    using InterceptorPOC.Tests.Targets;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class InterceptionRegistrationTypeTests
    {
        [Fact]
        public void InterfaceService_WithTypeRegistration_CallsInterceptor()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<IInterfaceWithAttributes, InterfaceWithAttributesImplementation>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target.MethodWithTestInterceptor();

            AssertHelper.Proxy(target);
            Assert.Equal(1, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void InterfaceService_WithFactoryRegistration_CallsInterceptor()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<IInterfaceWithAttributes>(
                    provider => new InterfaceWithAttributesImplementation(provider.GetRequiredService<Tracker>()))
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target.MethodWithTestInterceptor();

            AssertHelper.Proxy(target);
            Assert.Equal(1, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void InterfaceService_WithInstanceRegistration_CallsInterceptor()
        {
            var tracker = new Tracker();
            var testClassInstance = new InterfaceWithAttributesImplementation(tracker);
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<IInterfaceWithAttributes>(testClassInstance)
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target.MethodWithTestInterceptor();

            AssertHelper.Proxy(target);
            Assert.Equal(1, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithTypeRegistration_CallsInterceptor()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithAttributes>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target.MethodWithTestInterceptor();

            AssertHelper.Proxy(target);
            Assert.Equal(1, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithFactoryRegistration_CallsInterceptor()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton(provider => new ClassWithAttributes(provider.GetRequiredService<Tracker>()))
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target.MethodWithTestInterceptor();

            AssertHelper.Proxy(target);
            Assert.Equal(1, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithInstanceRegistration_CallsInterceptor()
        {
            var tracker = new Tracker();
            var testClassInstance = new ClassWithAttributes(tracker);
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton(testClassInstance)
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target.MethodWithTestInterceptor();

            AssertHelper.Proxy(target);
            Assert.Equal(1, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }
    }
}
