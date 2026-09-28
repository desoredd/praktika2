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
        int sum = 0;
        for (int i = 0;i < arr.Length; i++)
        {
            sum += arr[i];
            Console.WriteLine($"Сумма после {i+1} итерации = {sum}");
        }
        Console.WriteLine($"Сумма элементов массива = {sum}");
    }
}