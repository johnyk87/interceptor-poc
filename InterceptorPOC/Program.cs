namespace InterceptorPOC
{
    using System;
    using System.Threading.Tasks;
    using InterceptorPOC.Dependencies;
    using InterceptorPOC.Interceptors.Another;
    using InterceptorPOC.Interceptors.Some;
    using InterceptorPOC.Targets;
    using Microsoft.Extensions.DependencyInjection;

    public static class Program
    {
        public static async Task Main()
        {
            try
            {
                var services = new ServiceCollection()
                    .AddTransient<ITestClass, TestClass>()
                    .AddSingleton<SomeDependency>()
                    .AddSingleton<SomeInterceptor>()
                    .AddTransient<AnotherInterceptor>()
                    .AddAttributeInterception();

                var serviceProvider = services.BuildServiceProvider();

                var testClass = serviceProvider.GetRequiredService<ITestClass>();

                testClass.DoSomething(1);
                Console.WriteLine("DoSomething1");

                Console.WriteLine($"GetSomething2 = {testClass.GetSomething(2)}");

                Console.WriteLine($"EchoSomething3 = {testClass.EchoSomething(3)}");

                testClass = serviceProvider.GetRequiredService<ITestClass>();

                await testClass.DoSomethingAsync(1);
                Console.WriteLine("DoSomethingAsync1");

                Console.WriteLine($"GetSomethingAsync2 = {await testClass.GetSomethingAsync(2)}");

                Console.WriteLine($"EchoSomethingAsync3 = {await testClass.EchoSomethingAsync(3)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                Console.WriteLine($"StackTrace:{Environment.NewLine}{ex.StackTrace}");
            }

            Console.WriteLine("Press <ENTER> to terminate...");
            Console.ReadLine();
        }
    }
}
