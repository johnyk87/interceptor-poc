namespace InterceptorPOC.Targets
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class TestClass : ITestClass
    {
        public void DoSomething(int input)
        {
            Console.WriteLine($"Entered {nameof(TestClass)}.{nameof(this.DoSomething)} with input {input}.");

            Thread.Sleep(1000);

            Console.WriteLine($"Leaving {nameof(TestClass)}.{nameof(this.DoSomething)}");
        }

        public int GetSomething(int input)
        {
            Console.WriteLine($"Entered {nameof(TestClass)}.{nameof(this.GetSomething)} with input {input}.");

            Thread.Sleep(1000);

            Console.WriteLine($"Leaving {nameof(TestClass)}.{nameof(this.GetSomething)}");

            return input;
        }

        public T EchoSomething<T>(T input)
        {
            Console.WriteLine($"Entered {nameof(TestClass)}.{nameof(this.EchoSomething)} with input {input}.");

            Thread.Sleep(1000);

            Console.WriteLine($"Leaving {nameof(TestClass)}.{nameof(this.EchoSomething)}");

            return input;
        }

        public async Task DoSomethingAsync(int input)
        {
            Console.WriteLine($"Entered {nameof(TestClass)}.{nameof(this.DoSomethingAsync)} with input {input}.");

            await Task.Delay(1000);

            Console.WriteLine($"Leaving {nameof(TestClass)}.{nameof(this.DoSomethingAsync)}");
        }

        public async Task<int> GetSomethingAsync(int input)
        {
            Console.WriteLine($"Entered {nameof(TestClass)}.{nameof(this.GetSomethingAsync)} with input {input}.");

            await Task.Delay(1000);

            Console.WriteLine($"Leaving {nameof(TestClass)}.{nameof(this.GetSomethingAsync)}.");

            return input;
        }

        public async Task<T> EchoSomethingAsync<T>(T input)
        {
            Console.WriteLine($"Entered {nameof(TestClass)}.{nameof(this.EchoSomethingAsync)} with input {input}.");

            await Task.Delay(1000);

            Console.WriteLine($"Leaving {nameof(TestClass)}.{nameof(this.EchoSomethingAsync)}.");

            return input;
        }
    }
}
