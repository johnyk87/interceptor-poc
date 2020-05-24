namespace InterceptorPOC.Tests
{
    using System.Linq;
    using InterceptorPOC.Tests.Helpers;
    using InterceptorPOC.Tests.Interceptors;
    using InterceptorPOC.Tests.Targets;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class InterceptionOrderTests
    {
        [Fact]
        public void InterfaceService_WithMultipleAttributesInOrder_CallsInterceptorsInMethodDeclaredOrder()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<IInterfaceWithAttributes, InterfaceWithAttributesImplementation>()
                .AddSingleton<TestInterceptor>()
                .AddSingleton<AnotherTestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target.MethodWithMultipleTestInterceptors();

            AssertHelper.Proxy(target);
            Assert.Equal(2, tracker.InterceptorCalls);
            Assert.IsType<TestInterceptor>(tracker.InterceptorsCalled.ElementAt(0));
            Assert.IsType<AnotherTestInterceptor>(tracker.InterceptorsCalled.ElementAt(1));
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void InterfaceService_WithMultipleAttributesInInvertedOrder_CallsInterceptorsInMethodDeclaredOrder()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<IInterfaceWithAttributes, InterfaceWithAttributesImplementation>()
                .AddSingleton<TestInterceptor>()
                .AddSingleton<AnotherTestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target.MethodWithMultipleTestInterceptorsAndInvertedOrder();

            AssertHelper.Proxy(target);
            Assert.Equal(2, tracker.InterceptorCalls);
            Assert.IsType<AnotherTestInterceptor>(tracker.InterceptorsCalled.ElementAt(0));
            Assert.IsType<TestInterceptor>(tracker.InterceptorsCalled.ElementAt(1));
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithMultipleAttributesInOrder_CallsInterceptorsInMethodDeclaredOrder()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithAttributes>()
                .AddSingleton<TestInterceptor>()
                .AddSingleton<AnotherTestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target.MethodWithMultipleTestInterceptors();

            AssertHelper.Proxy(target);
            Assert.Equal(2, tracker.InterceptorCalls);
            Assert.IsType<TestInterceptor>(tracker.InterceptorsCalled.ElementAt(0));
            Assert.IsType<AnotherTestInterceptor>(tracker.InterceptorsCalled.ElementAt(1));
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithMultipleAttributesInInvertedOrder_CallsInterceptorsInMethodDeclaredOrder()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithAttributes>()
                .AddSingleton<TestInterceptor>()
                .AddSingleton<AnotherTestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target.MethodWithMultipleTestInterceptorsAndInvertedOrder();

            AssertHelper.Proxy(target);
            Assert.Equal(2, tracker.InterceptorCalls);
            Assert.IsType<AnotherTestInterceptor>(tracker.InterceptorsCalled.ElementAt(0));
            Assert.IsType<TestInterceptor>(tracker.InterceptorsCalled.ElementAt(1));
            Assert.Equal(1, tracker.TargetCalls);
        }
    }
}
