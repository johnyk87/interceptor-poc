namespace InterceptorPOC.Calculator
{
    using System;
    using System.Threading.Tasks;

    public class Calculator : ICalculator
    {
        public virtual int Increment(int input)
        {
            Console.WriteLine($"Entered {nameof(Calculator)}.{nameof(this.Increment)} with input {input}...");
            return input + 1;
        }

        public virtual int Decrement(int input)
        {
            Console.WriteLine($"Entered {nameof(Calculator)}.{nameof(this.Decrement)} with input {input}...");
            return input - 1;
        }

        public async Task<int> DelayedSumAsync(int leftParcel, int rightParcel)
        {
            Console.WriteLine($"Entered {nameof(Calculator)}.{nameof(this.DelayedSumAsync)} with inputs {leftParcel} and {rightParcel}...");

            await Task.Delay(1000);

            Console.WriteLine($"Now leaving {nameof(Calculator)}.{nameof(this.DelayedSumAsync)}...");

            return leftParcel + rightParcel;
        }

        public async Task DoSomethingAsync(int leftParcel, int rightParcel)
        {
            Console.WriteLine($"Entered {nameof(Calculator)}.{nameof(this.DoSomethingAsync)} with inputs {leftParcel} and {rightParcel}...");

            await Task.Delay(1000);

            Console.WriteLine($"Now leaving {nameof(Calculator)}.{nameof(this.DoSomethingAsync)}...");
        }

        public async Task<T> EchoSomethingAsync<T>(T returnValue)
        {
            Console.WriteLine($"Entered {nameof(Calculator)}.{nameof(this.EchoSomethingAsync)} with input {returnValue}...");

            await Task.Delay(1000);

            Console.WriteLine($"Now leaving {nameof(Calculator)}.{nameof(this.EchoSomethingAsync)}...");

            return returnValue;
        }
    }
}
