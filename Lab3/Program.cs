using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3
{
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Console.Write("N: ");
            var n = int.Parse(Console.ReadLine());
            Console.Write("K: ");
            var k = int.Parse(Console.ReadLine());
            Console.Write("p: ");
            var p = double.Parse(Console.ReadLine());
            Console.WriteLine("1 - time-based");
            Console.WriteLine("2 - iteration-based");
            Console.Write("mode: ");
            var mode = int.Parse(Console.ReadLine());
            Console.WriteLine();

            var crystal = new int[n];
            crystal[0] = k;
            for (var i = 1; i < crystal.Length; i++)
            {
                crystal[i] = 0;
            }

            ThreadPool.SetMinThreads(k, k);
            ThreadPool.SetMaxThreads(k, k);

            var timer = new Stopwatch();

            switch (mode)
            {
                case 1:
                    Console.Write("Time (seconds): ");
                    var time = int.Parse(Console.ReadLine());
                    Console.Write("Delay (seconds): ");
                    var delay = int.Parse(Console.ReadLine());

                    var source = new CancellationTokenSource();
                    var token = source.Token;

                    var factory = new TaskFactory(token);

                    timer.Start();
                    ShowCrystal(crystal, timer.Elapsed.TotalSeconds);

                    for (var i = 0; i < k; i++)
                    {
                        factory.StartNew(() =>
                        {
                            MoveParticle_Time(crystal, 0, delay, p);
                        },
                            token);
                    }

                    while (timer.Elapsed.TotalSeconds <= time)
                    {
                        ShowCrystal(crystal, timer.Elapsed.TotalSeconds);
                        Thread.Sleep(delay * 1000);
                    }

                    source.Cancel();
                    ShowCrystal(crystal, timer.Elapsed.TotalSeconds);
                    Console.WriteLine("End");
                    timer.Stop();
                    timer.Reset();

                    Console.ReadKey();

                    break;
                case 2:
                    Console.Write("Iterations: ");
                    var iterations = int.Parse(Console.ReadLine());

                    timer.Start();
                    for (var j = 0; j < k; j++)
                    {
                        Task.Run(() =>
                        {
                            MoveParticle_Iteration(crystal, 0, iterations, p);
                        });
                    }

                    ShowCrystal(crystal, timer.Elapsed.TotalSeconds);

                    timer.Stop();
                    timer.Reset();

                    Console.ReadKey();

                    break;
                default:
                    Console.WriteLine("O_o");
                    break;
            }

        }

        private static void MoveParticle_Time(IList<int> crystal, int cell, int delay, double p)
        {
            while (true)
            {
                MoveParticle(crystal, ref cell, p);

                Thread.Sleep(delay * 1000);
            }
        }

        private static void MoveParticle_Iteration(IList<int> crystal, int cell, int iterations, double p)
        {
            for (var i = 0; i < iterations; i++)
            {
                MoveParticle(crystal, ref cell, p);
            }
        }

        private static void MoveParticle(IList<int> crystal, ref int cell, double p)
        {
            var rnd = new Random();
            if (rnd.Next(0, 100) < p * 100)
            {
                if (cell == 0)
                    MoveRight(crystal, ref cell);
                else
                    MoveLeft(crystal, ref cell);
            }
            else
            {
                if (cell == crystal.Count - 1)
                    MoveLeft(crystal, ref cell);
                else
                    MoveRight(crystal, ref cell);
            }
        }

        private static void MoveLeft(IList<int> crystal, ref int cell)
        {
            lock (crystal)
                crystal[cell]--;
            cell--;
            lock (crystal)
                crystal[cell]++;
        }

        private static void MoveRight(IList<int> crystal, ref int cell)
        {
            lock (crystal)
                crystal[cell]--;
            cell++;
            lock (crystal)
                crystal[cell]++;
        }

        private static void ShowCrystal(ICollection<int> crystal, double totalSeconds)
        {
            Console.WriteLine("Crystal:");
            for (var i = 0; i <= crystal.Count; i++)
            {
                Console.Write("{0,-2}", "_");
            }
            Console.WriteLine();
            foreach (var item in crystal)
            {
                Console.Write("|" + item);
            }
            Console.Write("|");
            Console.WriteLine();
            for (var i = 0; i <= crystal.Count; i++)
            {
                Console.Write("{0,-2}", "\u0305");
            }
            Console.WriteLine();
            Console.WriteLine("Time: {0:F3}", totalSeconds);
            Console.WriteLine();
        }
    }
}
