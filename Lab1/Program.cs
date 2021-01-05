using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Program
    {
        private static void Main()
        {

        }

        private static void IO_bound(int n)
        {
            SetNumberOfThreads(n);

            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent;

            var tasks = new List<Task>();
            for (var i = 1; i <= n; i++)
            {
                tasks.Add(Task.Run(() => File.ReadAllText(path + "\\Hamlet.txt")));
            }

            Task.WaitAll(tasks.ToArray());

            // using Parallel library, same result
            //Parallel.For(0, n, i => File.ReadAllText(path + "\\Hamlet.txt"));
        }

        private static void SetNumberOfThreads(int n)
        {
            ThreadPool.SetMinThreads(n, n);
            ThreadPool.SetMaxThreads(n, n);
        }
    }
}
