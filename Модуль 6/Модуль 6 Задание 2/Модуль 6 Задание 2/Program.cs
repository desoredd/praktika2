using Модуль_6_Задание_2;
class Program
{
    static void Main()
    {
        DB db = new DB();
        bool run = true;
        while (run)
        {
            Console.WriteLine("1 - Просмотр задач");
            Console.WriteLine("2 - Добавить задачу");
            Console.WriteLine("3 - Изменить текст задачи");
            Console.WriteLine("4 - Удалить задачу");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");
            int chose = int.Parse(Console.ReadLine());
            switch (chose)
            {
                case 1:
                    Console.WriteLine($"\nВсе задачи:");
                    db.PrintAll();
                    break;
                case 2:
                    Console.Write("Введите название задачи: ");
                    string n = Console.ReadLine();
                    Console.Write("Введите текст задачи: ");
                    string t = Console.ReadLine();
                    db.Add(n, t);
                    break;
                case 3:
                    Console.WriteLine($"\nВсе задачи:");
                    db.PrintAll();
                    Console.Write($"Введите имя задачи для его изменения: ");
                    string nn = Console.ReadLine();
                    Console.Write("Введите текст задачи: ");
                    string te = Console.ReadLine();
                    db.Update(nn,te);
                    break;
                case 4:
                    Console.WriteLine($"\nВсе задачи:");
                    db.PrintAll();
                    Console.Write($"Введите имя задачи для его удаления:");
                    db.Remove(Console.ReadLine());
                    break;
                case 0:
                    run = false;
                    break;
            }
        }
    }
}