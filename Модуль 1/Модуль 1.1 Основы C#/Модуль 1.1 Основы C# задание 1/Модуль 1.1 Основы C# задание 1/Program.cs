class Program
{
    static void Qviz(int num)
    {
        Console.Write("Угадайте целочисленное число от 1 до 100: ");
        int manum = System.Convert.ToInt32(Console.ReadLine());
        while(true)
        {
            if (num == manum)
            {
                Console.WriteLine("Вы ввели заданное число");
                break;
            }
            else if (manum > num)
            {
                Console.WriteLine("Введенное число больше загаданного");
                Console.Write("Попытайтесь еще раз: ");
                manum = System.Convert.ToInt32(Console.ReadLine());
            }
            else if (manum < num)
            {
                Console.WriteLine("Введенное число меньше загаданного");
                Console.Write("Попытайтесь еще раз: ");
                manum = System.Convert.ToInt32(Console.ReadLine());
            }
        }
    }
    static void Main()
    {
        Random random = new Random();
        int num = random.Next(1, 101);
        Qviz(num);
    }
}