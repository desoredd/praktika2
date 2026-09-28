class Program
{
    static void Main()
    {
        Console.Write("Введите размер массива N: ");
        int n = System.Convert.ToInt32(Console.ReadLine());
        double[] arr = new double[n];
        for (int i = 0; i < n; i++)
        {
            Console.Write($"Введите элемент [{i}]: ");
            arr[i] = System.Convert.ToDouble(Console.ReadLine());
        }
        double max = 0;
        for (int i = 0; i < n; i++)
        {
            if (Math.Abs(arr[i]) > max)
                max = Math.Abs(arr[i]);
        }
        Console.WriteLine("\nНормированный массив:");
        for (int i = 0; i < n; i++)
        {
            arr[i] = arr[i] / max;
            Console.WriteLine(arr[i]);
        }
    }
}