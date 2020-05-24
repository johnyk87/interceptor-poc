namespace InterceptorPOC.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using InterceptorPOC.Tests.Helpers;
    using InterceptorPOC.Tests.Interceptors;
    using InterceptorPOC.Tests.Targets;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class InterceptorRegistrationLifetimeTests
    {
        [Fact]
        public void InterfaceService_WithSingletonTargetAndSingletonInterceptor_UsesSameInterceptorInstance()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<IInterfaceWithAttributes, InterfaceWithAttributesImplementation>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target1 = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();
            var target2 = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target1.MethodWithTestInterceptor();
            target2.MethodWithTestInterceptor();

            AssertHelper.Proxy(target1);
            AssertHelper.Proxy(target2);
            Assert.Equal(2, tracker.InterceptorCalls);
            Assert.Single(tracker.InterceptorsCalled.Distinct());
            Assert.Equal(2, tracker.TargetCalls);
            Assert.Single(tracker.TargetsCalled.Distinct());
        }

        [Fact]
        public void InterfaceService_WithTransientTargetAndSingletonInterceptor_UsesSameInterceptorInstance()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddTransient<IInterfaceWithAttributes, InterfaceWithAttributesImplementation>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target1 = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();
            var target2 = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target1.MethodWithTestInterceptor();
            target2.MethodWithTestInterceptor();

            AssertHelper.Proxy(target1);
            AssertHelper.Proxy(target2);
            Assert.Equal(2, tracker.InterceptorCalls);
            Assert.Single(tracker.InterceptorsCalled.Distinct());
            Assert.Equal(2, tracker.TargetCalls);
            Assert.Equal(2, tracker.TargetsCalled.Distinct().Count());
        }

        [Fact]
        public void InterfaceService_WithTransientTargetAndTransientInterceptor_UsesDifferentInterceptorInstances()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddTransient<IInterfaceWithAttributes, InterfaceWithAttributesImplementation>()
                .AddTransient<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target1 = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();
            var target2 = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target1.MethodWithTestInterceptor();
            target2.MethodWithTestInterceptor();

            AssertHelper.Proxy(target1);
            AssertHelper.Proxy(target2);
            Assert.Equal(2, tracker.InterceptorCalls);
            Assert.Equal(2, tracker.InterceptorsCalled.Distinct().Count());
            Assert.Equal(2, tracker.TargetCalls);
            Assert.Equal(2, tracker.TargetsCalled.Distinct().Count());
        }

        [Fact]
        public void InterfaceService_WithTransientInterceptorWithTransientDependency_UsesDifferentDependencyInstances()
        {
            var trackers = new List<Tracker>();
            var serviceProvider = new ServiceCollection()
                .AddTransient(
                    provider =>
                    {
                        var tracker = new Tracker();
                        trackers.Add(tracker);
                        return tracker;
                    })
                .AddTransient<IInterfaceWithAttributes, InterfaceWithAttributesImplementation>()
                .AddTransient<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target1 = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();
            var target2 = serviceProvider.GetRequiredService<IInterfaceWithAttributes>();

            target1.MethodWithTestInterceptor();
            target2.MethodWithTestInterceptor();

            AssertHelper.Proxy(target1);
            AssertHelper.Proxy(target2);
            Assert.Equal(4, trackers.Count);
            Assert.Equal(2, trackers.Sum(tracker => tracker.InterceptorCalls));
            Assert.Equal(2, trackers.SelectMany(tracker => tracker.InterceptorsCalled).Distinct().Count());
            Assert.Equal(2, trackers.Sum(tracker => tracker.TargetCalls));
            Assert.Equal(2, trackers.SelectMany(tracker => tracker.TargetsCalled).Distinct().Count());
        }

        [Fact]
        public void ClassService_WithSingletonTargetAndSingletonInterceptor_UsesSameInterceptorInstance()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddSingleton<ClassWithAttributes>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target1 = serviceProvider.GetRequiredService<ClassWithAttributes>();
            var target2 = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target1.MethodWithTestInterceptor();
            target2.MethodWithTestInterceptor();

            AssertHelper.Proxy(target1);
            AssertHelper.Proxy(target2);
            Assert.Equal(2, tracker.InterceptorCalls);
            Assert.Single(tracker.InterceptorsCalled.Distinct());
            Assert.Equal(2, tracker.TargetCalls);
            Assert.Single(tracker.TargetsCalled.Distinct());
        }

        [Fact]
        public void ClassService_WithTransientTargetAndSingletonInterceptor_UsesSameInterceptorInstance()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddTransient<ClassWithAttributes>()
                .AddSingleton<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target1 = serviceProvider.GetRequiredService<ClassWithAttributes>();
            var target2 = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target1.MethodWithTestInterceptor();
            target2.MethodWithTestInterceptor();

            AssertHelper.Proxy(target1);
            AssertHelper.Proxy(target2);
            Assert.Equal(2, tracker.InterceptorCalls);
            Assert.Single(tracker.InterceptorsCalled.Distinct());
            Assert.Equal(2, tracker.TargetCalls);
            Assert.Equal(2, tracker.TargetsCalled.Distinct().Count());
        }

        [Fact]
        public void ClassService_WithTransientTargetAndTransientInterceptor_UsesDifferentInterceptorInstances()
        {
            var tracker = new Tracker();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(tracker)
                .AddTransient<ClassWithAttributes>()
                .AddTransient<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target1 = serviceProvider.GetRequiredService<ClassWithAttributes>();
            var target2 = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target1.MethodWithTestInterceptor();
            target2.MethodWithTestInterceptor();

            AssertHelper.Proxy(target1);
            AssertHelper.Proxy(target2);
            Assert.Equal(2, tracker.InterceptorCalls);
            Assert.Equal(2, tracker.InterceptorsCalled.Distinct().Count());
            Assert.Equal(2, tracker.TargetCalls);
            Assert.Equal(2, tracker.TargetsCalled.Distinct().Count());
        }

        [Fact]
        public void ClassService_WithTransientInterceptorWithTransientDependency_UsesDifferentDependencyInstances()
        {
            var trackers = new List<Tracker>();
            var serviceProvider = new ServiceCollection()
                .AddTransient(
                    provider =>
                    {
                        var tracker = new Tracker();
                        trackers.Add(tracker);
                        return tracker;
                    })
                .AddTransient<ClassWithAttributes>()
                .AddTransient<TestInterceptor>()
                .AddAttributeInterception()
                .BuildServiceProvider();
            var target1 = serviceProvider.GetRequiredService<ClassWithAttributes>();
            var target2 = serviceProvider.GetRequiredService<ClassWithAttributes>();

            target1.MethodWithTestInterceptor();
            target2.MethodWithTestInterceptor();

            AssertHelper.Proxy(target1);
            AssertHelper.Proxy(target2);
            Assert.Equal(4, trackers.Count);
            Assert.Equal(2, trackers.Sum(tracker => tracker.InterceptorCalls));
            Assert.Equal(2, trackers.SelectMany(tracker => tracker.InterceptorsCalled).Distinct().Count());
            Assert.Equal(2, trackers.Sum(tracker => tracker.TargetCalls));
            Assert.Equal(2, trackers.SelectMany(tracker => tracker.TargetsCalled).Distinct().Count());
        }
    }
}
