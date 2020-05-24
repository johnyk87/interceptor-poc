namespace InterceptorPOC.Tests
{
    using System;
    using System.Linq;
    using Castle.DynamicProxy;
    using InterceptorPOC.Tests.Helpers;
    using InterceptorPOC.Tests.Interceptors;
    using InterceptorPOC.Tests.Targets;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class AttributeInterceptionTests
    {
        [Fact]
        public void InterfaceService_WithAttributeAndRegisteredInterceptor_CallsInterceptor()
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
        public void InterfaceService_WithAttributeAndInterceptorNotRegistered_DoesNotIntercept()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<IInterfaceWithAttributes, InterfaceWithAttributesImplementation>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target.MethodWithTestInterceptor();

            AssertHelper.NotProxy(target);
            Assert.Equal(0, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void InterfaceService_WithMultipleAttributesAndAllInterceptorsRegistered_CallsAllInterceptors()
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
            Assert.Equal(2, tracker.InterceptorsCalled.Distinct().Count());
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void InterfaceService_WithMultipleAttributesAndOnlyOneInterceptorRegistered_CallsOneInterceptor()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<IInterfaceWithAttributes, InterfaceWithAttributesImplementation>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target.MethodWithMultipleTestInterceptors();

            AssertHelper.Proxy(target);
            Assert.Equal(1, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void InterfaceService_WithMethodWithoutAttribute_DoesNotIntercept()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<IInterfaceWithAttributes, InterfaceWithAttributesImplementation>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target.MethodWithoutAttribute();

            AssertHelper.Proxy(target);
            Assert.Equal(0, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void InterfaceService_WithoutAttributes_DoesNotIntercept()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<IInterfaceWithoutAttributes, InterfaceWithoutAttributesImplementation>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<IInterfaceWithoutAttributes>();

            target.MethodWithoutAttribute();

            AssertHelper.NotProxy(target);
            Assert.Equal(0, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithAttributeAndRegisteredInterceptor_CallsInterceptor()
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
        public void ClassService_WithAttributeAndInterceptorNotRegistered_DoesNotIntercept()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithAttributes>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target.MethodWithTestInterceptor();

            AssertHelper.NotProxy(target);
            Assert.Equal(0, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithMultipleAttributesAndAllInterceptorsRegistered_CallsAllInterceptors()
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
            Assert.Equal(2, tracker.InterceptorsCalled.Distinct().Count());
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithMultipleAttributesAndOnlyOneInterceptorRegistered_CallsOneInterceptor()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithAttributes>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target.MethodWithMultipleTestInterceptors();

            AssertHelper.Proxy(target);
            Assert.Equal(1, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithMethodWithoutAttribute_DoesNotIntercept()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithAttributes>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target.MethodWithoutAttribute();

            AssertHelper.Proxy(target);
            Assert.Equal(0, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithoutAttributes_DoesNotIntercept()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithoutAttributes>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithoutAttributes>();

            target.MethodWithoutAttribute();

            AssertHelper.NotProxy(target);
            Assert.Equal(0, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        public void ClassService_WithNonVirtualMethodWithAttribute_ThrowsException()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithInterceptedNonVirtualMethod>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();

            Assert.Throws<InvalidOperationException>(
                () => serviceProvider.GetRequiredService<ClassWithInterceptedNonVirtualMethod>());
        }

        [Fact]
        public void ClassService_WithNonVirtualMethodWithoutAttribute_ThrowsException()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithNonInterceptedNonVirtualPublicMethod>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();

            Assert.Throws<InvalidOperationException>(
                () => serviceProvider.GetRequiredService<ClassWithNonInterceptedNonVirtualPublicMethod>());
        }

        [Fact]
        public void ClassService_WithoutParameterlessConstructor_ThrowsException()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithoutParameterlessConstructor>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();

            Assert.Throws<InvalidProxyConstructorArgumentsException>(
                () => serviceProvider.GetRequiredService<ClassWithoutParameterlessConstructor>());
        }

        [Fact]
        /*
         * WARN: Private members are not overridable, and are not even considered by the proxy generator.
         * Because of this they can't be intercepted when using target based proxies.
         */
        public void ClassService_WithPrivateMethodWithAttribute_DoesNotIntercept()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithInterceptedPrivateMethod>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithInterceptedPrivateMethod>();

            target.MethodWithoutAttribute();

            AssertHelper.NotProxy(target);
            Assert.Equal(0, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }

        [Fact]
        /*
         * WARN: When using target based proxies, protected members are called in the context of the target
         * itself, not the proxy. Because of this, protected members can't be intercepted reliably.
         */
        public void ClassService_WithProtectedMethodWithAttribute_DoesNotIntercept()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithInterceptedProtectedMethod>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target = serviceProvider.GetRequiredService<ClassWithInterceptedProtectedMethod>();

            target.MethodWithoutAttribute();

            AssertHelper.NotProxy(target);
            Assert.Equal(0, tracker.InterceptorCalls);
            Assert.Equal(1, tracker.TargetCalls);
        }
    }
}
