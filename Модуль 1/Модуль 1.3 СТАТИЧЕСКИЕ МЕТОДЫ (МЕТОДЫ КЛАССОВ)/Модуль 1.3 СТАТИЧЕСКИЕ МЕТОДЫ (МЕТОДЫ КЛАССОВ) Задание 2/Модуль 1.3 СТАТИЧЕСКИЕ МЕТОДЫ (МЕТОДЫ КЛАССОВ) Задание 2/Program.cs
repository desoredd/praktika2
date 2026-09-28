class Program
{
    static void Main()
    {
        Console.Write("Введите предельную сумму: ");
        int lim = System.Convert.ToInt16(Console.ReadLine());
        Random r = new Random();
        int sum = 0;
        int count = 0;
        int[] a = new int[100];
        while (true)
        {
            int next = r.Next(1, 10);
            if (sum + next > lim) break;
            a[count++] = next;
            sum += next;
        }
        int[] res = new int[count];
        Array.Copy(a, res, count);
        Console.WriteLine($"\nМассив из {count} элементов (сумма = {sum}):");
        for (int i = 0; i < res.Length; i++)
        {
            Console.Write(res[i] + " ");
        }    
        Console.WriteLine();
    }
}