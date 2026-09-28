class Program
{
    static void Main()
    {
        Console.Write("Введите количество элементов K: ");
        int k = System.Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите начало диапазона A: ");
        int a = System.Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите конец диапазона B: ");
        int b = System.Convert.ToInt32(Console.ReadLine());
        int[] arr = new int[k];
        Random rnd = new Random();
        for (int i = 0; i < k; i++)
        {
            arr[i] = rnd.Next(a, b);
        }
        Console.WriteLine("\nИсходный массив:");
        for (int i = 0; i < k; i++)
        {
            Console.Write(arr[i] + " ");
        }
        Console.WriteLine();
        int min = 0;
        int max = 0;

        for (int i = 1; i < k; i++)
        {
            if (arr[i] < arr[min]) min = i;
            if (arr[i] > arr[max]) max = i;
        }
        Console.WriteLine($"\nИндекс минимального элемента: {min} (значение {arr[min]})");
        Console.WriteLine($"Индекс максимального элемента: {max}  (значение  {arr[max]})");
        int st = Math.Min(min, max);
        int e = Math.Max(min, max);
        Console.WriteLine($"\nЭлементы с {st} по {e} включительно:");
        for (int i = st; i <= e; i++)
        {
            Console.Write(arr[i] + " ");
        }
        Console.WriteLine();
    }
}