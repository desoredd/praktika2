class Program
{
    static int GCM(int a, int b)
    {
        while (b != 0)
        {
            int t = b;
            b = a % b;
            a = t;
        }
        return a;
    }
    static void Main()
    {
        Console.Write("Введите числитель: ");
        int num = System.Convert.ToInt16(Console.ReadLine());
        Console.Write("Введите знаменатель: ");
        int den = System.Convert.ToInt16(Console.ReadLine());
        int g = GCM(num, den);
        Console.WriteLine($"Сокращённая дробь: {num / g}/{den / g}");
    }
}