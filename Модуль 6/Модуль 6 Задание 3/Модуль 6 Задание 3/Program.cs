using Модуль_6_Задание_3;
class Program
{
    static void Main()
    {
        DB db = new DB();
        bool run = true;
        while (run)
        {
            Console.WriteLine("1 - Просмотр всех книг");
            Console.WriteLine("2 - Добавить книгу");
            Console.WriteLine("3 - Изменить данные книги");
            Console.WriteLine("4 - Удалить книгу");
            Console.WriteLine("5 - Фильтр по названию");
            Console.WriteLine("6 - Фильтр по автору");
            Console.WriteLine("7 - Фильтр по жанру");
            Console.WriteLine("8 - Фильтр по году выпуска");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");
            int chose = int.Parse(Console.ReadLine());
            switch (chose)
            {
                case 1:
                    Console.WriteLine($"\nВсе книги:");
                    db.PrintAll();
                    break;
                case 2:
                    Console.Write("Введите название книги: ");
                    string name = Console.ReadLine();
                    Console.Write("Введите автора: ");
                    string author = Console.ReadLine();
                    Console.Write("Введите жанр: ");
                    string genre = Console.ReadLine();
                    Console.Write("Введите год выпуска: ");
                    int vp = int.Parse(Console.ReadLine());
                    db.Add(name, author, genre, vp);
                    break;
                case 3:
                    Console.WriteLine($"\nВсе книги:");
                    db.PrintAll();
                    Console.Write("Введите название книги для изменения: ");
                    string updName = Console.ReadLine();
                    Console.Write("Введите нового автора: ");
                    string newAuthor = Console.ReadLine();
                    Console.Write("Введите новый жанр: ");
                    string newGenre = Console.ReadLine();
                    Console.Write("Введите новый год выпуска: ");
                    int newVp = int.Parse(Console.ReadLine());
                    db.Update(updName, newAuthor, newGenre, newVp);
                    break;
                case 4:
                    Console.WriteLine($"\nВсе книги:");
                    db.PrintAll();
                    Console.Write("Введите название книги для удаления: ");
                    db.Remove(Console.ReadLine());
                    break;
                case 5:
                    Console.Write("Введите название книги: ");
                    db.FilterByName(Console.ReadLine());
                    break;
                case 6:
                    Console.Write("Введите автора: ");
                    db.FilterByAuthor(Console.ReadLine());
                    break;
                case 7:
                    Console.Write("Введите жанр: ");
                    db.FilterByGenre(Console.ReadLine());
                    break;
                case 8:
                    Console.Write("Введите год выпуска: ");
                    db.FilterByVp(int.Parse(Console.ReadLine()));
                    break;
                case 0:
                    run = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
}