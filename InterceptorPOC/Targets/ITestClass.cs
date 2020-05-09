namespace InterceptorPOC.Targets
{
    using InterceptorPOC.Interceptors.Async;
    using InterceptorPOC.Interceptors.Sync;
    using System.Threading.Tasks;

    public interface ITestClass
    {
        [Sync("DoSomething sync")]
        [Async("DoSomething async")]
        void DoSomething(int input);

        [Sync("GetSomething sync")]
        [Async("GetSomething async")]
        int GetSomething(int input);

        [Async("EchoSomething async")]
        [Sync("EchoSomething sync")]
        T EchoSomething<T>(T input);

        [Sync("DoSomethingAsync sync")]
        [Async("DoSomethingAsync async")]
        Task DoSomethingAsync(int input);

        [Sync("GetSomethingAsync sync")]
        [Async("GetSomethingAsync async")]
        Task<int> GetSomethingAsync(int input);

        [Async("EchoSomethingAsync async")]
        [Sync("EchoSomethingAsync sync")]
        Task<T> EchoSomethingAsync<T>(T input);
    }
}
