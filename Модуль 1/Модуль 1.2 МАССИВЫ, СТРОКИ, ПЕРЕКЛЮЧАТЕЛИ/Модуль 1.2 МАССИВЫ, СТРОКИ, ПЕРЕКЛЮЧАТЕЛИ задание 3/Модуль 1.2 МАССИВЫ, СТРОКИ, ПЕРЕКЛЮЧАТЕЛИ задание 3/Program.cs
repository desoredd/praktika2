class Program
{
    static void Main()
    {
        Console.Write("Введите количество простых чисел K: ");
        int k = System.Convert.ToInt32(Console.ReadLine());
        int posled = 0;
        int number = 2;
        while (posled < k)
        {
            if (Prost(number))
            {
                Console.Write(number + " ");
                posled++;
            }
            if (posled % 10 == 0)
            {
                Console.WriteLine();
            }
            number++;
        }
    }
    static bool Prost(int n)
    {
        for (int i = 2; i * i <= n; i++)
        {
            if (n % i == 0)
                return false;
        }
        return true;
    }
}