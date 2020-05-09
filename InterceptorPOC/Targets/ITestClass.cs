namespace InterceptorPOC.Targets
{
    using InterceptorPOC.Interceptors.Another;
    using InterceptorPOC.Interceptors.Some;
    using System.Threading.Tasks;

    public interface ITestClass
    {
        [Some("DoSomething some")]
        [Another("DoSomething another")]
        void DoSomething(int input);

        [Some("GetSomething some")]
        [Another("GetSomething another")]
        int GetSomething(int input);

        [Another("EchoSomething another")]
        [Some("EchoSomething some")]
        T EchoSomething<T>(T input);

        [Some("DoSomethingAsync some")]
        [Another("DoSomethingAsync another")]
        Task DoSomethingAsync(int input);

        [Some("GetSomethingAsync some")]
        [Another("GetSomethingAsync another")]
        Task<int> GetSomethingAsync(int input);

        [Another("EchoSomethingAsync another")]
        [Some("EchoSomethingAsync some")]
        Task<T> EchoSomethingAsync<T>(T input);
    }
}
