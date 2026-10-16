using Модуль_6_Задание_1;
class Program
{
    static void Main()
    {
        DB db = new DB();
        bool run = true;
        while (run)
        {
            Console.WriteLine("1 - Просмотр сотрудников");
            Console.WriteLine("2 - Добавить сотрудника");
            Console.WriteLine("3 - Удалить сотрудника");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");
            int chose = int.Parse(Console.ReadLine());
            switch( chose )
            {
                case 1:
                    Console.WriteLine($"\nВсе сотрудники:");
                    db.PrintAll();
                break;
                case 2:
                    Console.Write("Имя нового сотрудника: ");
                    string n = Console.ReadLine();
                    Console.Write("Должность нового сотрудника: ");
                    string dol = Console.ReadLine();
                    db.Add(n, dol);
                    break;
                case 3:
                    Console.WriteLine($"\nВсе сотрудники:");
                    db.PrintAll();
                    Console.Write($"Введите имя сотрудника для его удаления:");
                    db.Remove(Console.ReadLine());
                break;
                case 0:
                    run = false;
                break;
            }  
        }
    }
}