class Program
{
    static void Main()
    {
        Console.Write("Введите первое целоцисленное число: ");
        int num1 = System.Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите второе целоцисленное число: ");
        int num2 = System.Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите третье целоцисленное число: ");
        int num3 = System.Convert.ToInt32(Console.ReadLine());
        double arif = (num1 + num2 + num3) / 3;
        Console.WriteLine($"Среднее арифметическое данных чисел = {arif}");
    }
}