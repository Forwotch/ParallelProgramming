using System;
using System.Threading.Tasks;

namespace Lab2
{
    internal class Program
    {
        private static void Main()
        {
            var matrix1 = new int[10, 10];
            var matrix2 = new int[10, 10];
            var matrix3 = new int[10, 10];

            GenerateMatrix(matrix1);
            GenerateMatrix(matrix2);

            var n = matrix1.GetLength(0);
            var r = matrix1.GetLength(1);
            var m = matrix2.GetLength(0);

            for (var a = 0; a < n; a++)
            {
                for (var b = 0; b < r; b++)
                {
                    var i = a;
                    var j = b;

                    Task.Run(() =>
                    {
                        var sum = 0;
                        for (var k = 0; k < m; k++)
                        {
                            sum += matrix1[i, k] + matrix2[k, j];
                        }
                        matrix3[i, j] = sum;
                    });
                }
            }

            Console.WriteLine("Matrix 1:");
            ShowMatrix(matrix1);
            Console.WriteLine("Matrix 2:");
            ShowMatrix(matrix2);
            Console.WriteLine("Matrix 3:");
            ShowMatrix(matrix3);

            var min = (0, 0);
            for (var i = 0; i < matrix3.GetLength(0); i++)
            {
                for (var j = 0; j < matrix3.GetLength(1); j++)
                {
                    if (matrix3[min.Item1, min.Item2] > matrix3[i, j])
                    {
                        min = (i, j);
                    }
                }
            }

            Console.WriteLine("Min element:");
            Console.WriteLine($"matrix[{min.Item1 + 1}, {min.Item2 + 1}] = {matrix3[min.Item1, min.Item2]}");

            Console.ReadKey();
        }

        private static void GenerateMatrix(int[,] matrix)
        {
            var rnd = new Random();

            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = rnd.Next(-10, 10);
                }
            }
        }

        private static void ShowMatrix(int[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0,4}", matrix[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
