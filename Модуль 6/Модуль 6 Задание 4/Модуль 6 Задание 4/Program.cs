using Модуль_6_Задание_4;
class Program
{
    static void Main()
    {
        DB db = new DB();
        bool run = true;
        while (run)
        {
            Console.WriteLine("\nУЧЁТ ФИНАНСОВ");
            Console.WriteLine("1  - Показать все записи");
            Console.WriteLine("2  - Добавить доход");
            Console.WriteLine("3  - Добавить расход");
            Console.WriteLine("4  - Редактировать запись по ID");
            Console.WriteLine("5  - Удалить запись по ID");
            Console.WriteLine("6  - Показать итоги (доход/расход/баланс)");
            Console.WriteLine("7  - Фильтр по типу (доход/расход)");
            Console.WriteLine("8  - Фильтр по категории");
            Console.WriteLine("9  - Фильтр по дате");
            Console.WriteLine("0  - Выход");
            Console.Write("Ваш выбор: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Неверный ввод.");
                continue;
            }
            switch (choice)
            {
                case 1:
                    Console.WriteLine("\nВсе записи:");
                    db.PrintAll();
                    break;
                case 2:
                    AddTransaction(db, "income");
                    break;
                case 3:
                    AddTransaction(db, "expense");
                    break;
                case 4:
                    Console.WriteLine("\nТекущие записи:");
                    db.PrintAll();
                    Console.Write("Введите ID записи для редактирования: ");
                    if (!int.TryParse(Console.ReadLine(), out int editId))
                    {
                        Console.WriteLine("Неверный ID.");
                        break;
                    }
                    Console.Write("Тип (1 - Доход, 2 - Расход): ");
                    string typeChoice = Console.ReadLine();
                    string editType = typeChoice == "1" ? "income" : "expense";
                    Console.Write("Категория: ");
                    string editCat = Console.ReadLine();
                    Console.Write("Сумма: ");
                    double editAmount = double.Parse(Console.ReadLine());
                    Console.Write("Дата (YYYY-MM-DD): ");
                    string editDate = Console.ReadLine();
                    Console.Write("Комментарий: ");
                    string editComment = Console.ReadLine();
                    db.Update(editId, editType, editCat, editAmount, editDate, editComment);
                    break;
                case 5:
                    Console.WriteLine("\nТекущие записи:");
                    db.PrintAll();
                    Console.Write("Введите ID записи для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int delId))
                        db.Remove(delId);
                    else
                        Console.WriteLine("Неверный ID.");
                    break;
                case 6:
                    Console.WriteLine("\nИтоги:");
                    db.PrintSummary();
                    break;
                case 7:
                    Console.Write("Введите тип (1 - Доход, 2 - Расход): ");
                    string fType = Console.ReadLine() == "1" ? "income" : "expense";
                    db.FilterByType(fType);
                    break;
                case 8:
                    Console.Write("Введите категорию: ");
                    db.FilterByCategory(Console.ReadLine());
                    break;
                case 9:
                    Console.Write("Введите дату (YYYY-MM-DD): ");
                    db.FilterByDate(Console.ReadLine());
                    break;
                case 0:
                    run = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
    // Общая функция добавления дохода/расхода
    static void AddTransaction(DB db, string type)
    {
        Console.Write("Категория: ");
        string category = Console.ReadLine();
        Console.Write("Сумма: ");
        double amount = double.Parse(Console.ReadLine());
        Console.Write("Дата (YYYY-MM-DD, Enter — сегодня): ");
        string date = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(date))
            date = DateTime.Now.ToString("yyyy-MM-dd");
        Console.Write("Комментарий (необязательно): ");
        string comment = Console.ReadLine();
        db.Add(type, category, amount, date, comment);
        Console.WriteLine("Запись добавлена.");
    }
}