class Program
{
    static void Main()
    {
        Random rand = new Random();
        int[] arr = new int[10];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = rand.Next(100);
        }
        Console.WriteLine("исходный массив");
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write($"{arr[i]} ");
        }
        int max = 0;
        int id = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (max < arr[i])
            {
                max = arr[i];
                id = i;
            }
        }
        Console.WriteLine();
        Console.Write("Введите число на которое заменится максимальный элемент массива: ");
        arr[id] = System.Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("измененный массив");
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write($"{arr[i]} ");
        }
    }
}