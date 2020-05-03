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

        [Another("ICalculator.Decrementer another")]
        [Some("ICalculator.Decrementer some")]
        Task<int> DelayedSumAsync(int leftParcel, int rightParcel);
    }
}
