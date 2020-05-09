namespace InterceptorPOC
{
    using System;
    using System.Threading.Tasks;
    using InterceptorPOC.Calculator;
    using InterceptorPOC.Dependencies;
    using InterceptorPOC.Interceptors.Another;
    using InterceptorPOC.Interceptors.Some;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var services = new ServiceCollection()
                    .AddTransient<ICalculator, Calculator.Calculator>()
                    .AddSingleton<SomeDependency>()
                    .AddSingleton<SomeInterceptor>()
                    .AddTransient<AnotherInterceptor>()
                    .AddAttributeInterceptors();

                var serviceProvider = services.BuildServiceProvider();

                var calculator = serviceProvider.GetRequiredService<ICalculator>();

                Console.WriteLine($"Increment1 = {calculator.Increment(1)}");
                Console.WriteLine($"Decrement1 = {calculator.Decrement(1)}");

                calculator = serviceProvider.GetRequiredService<ICalculator>();

                Console.WriteLine($"Increment2 = {calculator.Increment(2)}");
                Console.WriteLine($"Decrement2 = {calculator.Decrement(2)}");

                Console.WriteLine($"Sum1&2 = {await calculator.DelayedSumAsync(1, 2)}");

                await calculator.DoSomethingAsync(1, 2);
                Console.WriteLine("Something1&2");

                Console.WriteLine($"Echo1 = {await calculator.EchoSomethingAsync(1)}");
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
