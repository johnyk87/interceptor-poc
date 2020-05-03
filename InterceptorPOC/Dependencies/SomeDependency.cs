namespace InterceptorPOC.Dependencies
{
    using System;

    public class SomeDependency
    {
        public void Before(string name)
        {
            Console.WriteLine($"Before {name}...");
        }

        public void After(string name)
        {
            Console.WriteLine($"After {name}...");
        }

        public void Catch(string name, Exception exception)
        {
            Console.WriteLine($"Catch {name} [{exception.GetType().Name}: {exception.Message}]...");
        }

        public void Finally(string name)
        {
            Console.WriteLine($"Finally {name}...");
        }
    }
}
