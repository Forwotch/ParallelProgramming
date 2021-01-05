﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private static double CPU_bound(int n, int angle)
        {
            SetNumberOfThreads(n);

            var tasks = new List<Task<double>>();
            for (var i = 1; i < 1000; i++)
            {
                var k = i;
                tasks.Add(Task.Run(() => Math.Pow(-1, k - 1) / Factorial(2 * k - 1) * Math.Pow(angle, 2 * k - 1)));
            }

            Task.WaitAll(tasks.ToArray());

            return tasks.Sum(task => task.Result);
        }

        private static int Memory_bound(int n, int number)
        {
            SetNumberOfThreads(n);

            var array = new int[number];
            for (var i = 0; i < number; i++)
            {
                array[i] = -1;
            }

            return Fibonacci(array, number);
        }

        private static int Fibonacci(IList<int> results, int n)
        {
            int val;

            if (n <= 0)
            {
                return 0;
            }

            if (results[n - 1] != -1)
                return results[n - 1];

            switch (n)
            {
                case 0:
                    val = 0;
                    break;
                case 1:
                    val = 1;
                    break;
                default:
                    val = Task.Run(() => Fibonacci(results, n - 2)).Result +
                          Task.Run(() => Fibonacci(results, n - 1)).Result;
                    break;
            }

            results[n - 1] = val;

            return val;
        }

        private static int Factorial(int n)
        {
            int i, fact = 1;
            for (i = 1; i <= n; i++)
            {
                fact = fact * i;
            }
            return fact;
        }

        private static void SetNumberOfThreads(int n)
        {
            ThreadPool.SetMinThreads(n, n);
            ThreadPool.SetMaxThreads(n, n);
        }
    }
}
