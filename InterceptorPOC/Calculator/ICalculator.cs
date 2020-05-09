namespace InterceptorPOC.Calculator
{
    using InterceptorPOC.Interceptors.Another;
    using InterceptorPOC.Interceptors.Some;
    using System.Threading.Tasks;

    public interface ICalculator
    {
        [Some("ICalculator.Incrementer some")]
        [Another("ICalculator.Incrementer another")]
        int Increment(int input);

        [Another("ICalculator.Decrementer another")]
        [Some("ICalculator.Decrementer some")]
        int Decrement(int input);

        [Another("ICalculator.DelayedSumAsync another")]
        [Some("ICalculator.DelayedSumAsync some")]
        Task<int> DelayedSumAsync(int leftParcel, int rightParcel);

        [Another("ICalculator.DoSomethingAsync another")]
        [Some("ICalculator.DoSomethingAsync some")]
        Task DoSomethingAsync(int leftParcel, int rightParcel);

        [Another("ICalculator.EchoSomethingAsync another")]
        [Some("ICalculator.EchoSomethingAsync some")]
        Task<T> EchoSomethingAsync<T>(T returnValue);
    }
}
