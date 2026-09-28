class Program
{
    static void Main()
    {
        Console.Write("Введите первую строку: ");
        string str1 = Console.ReadLine();
        Console.Write("Введите вторую строку: ");
        string str2 = Console.ReadLine();
        bool sovpadenie = str1.Contains(str2);
        Console.WriteLine($"Вторая строка является подстрокой первой: {sovpadenie}");
    }
}