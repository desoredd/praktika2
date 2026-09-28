class Program
{
    static void Main()
    {
        double[] a = new double[10];
        Random r = new Random();
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = r.NextDouble() * 20 - 10;
        }
        int[] idx = new int[a.Length];
        for (int i = 0; i < idx.Length; i++)
        {
            idx[i] = i;
        }
        for (int i = 0; i < idx.Length - 1; i++)
        {
            for (int j = 0; j < idx.Length - 1 - i; j++)
            {
                if (a[idx[j]] > a[idx[j + 1]])
                {
                    int t = idx[j];
                    idx[j] = idx[j + 1];
                    idx[j + 1] = t;
                }
            }   
        }
        Console.WriteLine("Массив a:");
        for (int i = 0; i < a.Length; i++)
        {
            Console.WriteLine($"a[{i}] = {a[i]:F3}");
        }
        Console.WriteLine("Индексы в порядке возрастания значений:");
        for (int i = 0; i < idx.Length; i++)
        {
            Console.WriteLine($"{idx[i]} (a[{idx[i]}] = {a[idx[i]]:F3})");
        }
    }
}