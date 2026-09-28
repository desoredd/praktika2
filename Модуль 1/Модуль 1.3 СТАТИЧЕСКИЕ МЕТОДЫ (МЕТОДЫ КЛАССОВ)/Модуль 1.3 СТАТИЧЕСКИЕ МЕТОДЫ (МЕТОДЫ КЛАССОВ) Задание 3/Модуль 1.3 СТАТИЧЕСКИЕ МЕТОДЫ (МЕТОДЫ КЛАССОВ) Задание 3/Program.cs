using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите колличество столбцов в квадратичной матрице: ");
        int n = System.Convert.ToInt16(Console.ReadLine());
        int[,] m = new int[n, n];
        Random r = new Random();

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                m[i, j] = r.Next(-50, 51);

        int[] sums = new int[n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                sums[i] += m[i, j];

        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - 1 - i; j++)
                if (sums[j] > sums[j + 1])
                {
                    int t = sums[j]; sums[j] = sums[j + 1]; sums[j + 1] = t;
                    for (int k = 0; k < n; k++)
                    {
                        t = m[j, k]; m[j, k] = m[j + 1, k]; m[j + 1, k] = t;
                    }
                }

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
                Console.Write($"{m[i, j],5}");
            Console.WriteLine();
        }
    }
}