// Класс книги
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public Book(string title, string author, int year, string genre = "—")
    {
        Title = title;
        Author = author;
        Year = year;
        Genre = genre;
    }
    public override string ToString() =>
        $"\"{Title}\" — {Author}, {Year} г., жанр: {Genre}";
}

// Класс библиотеки с фильтрацией
public class Library
{
    private readonly List<Book> _books = new();
    public void Add(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));
        _books.Add(book);
    }
    public IReadOnlyList<Book> Books => _books;
    public void PrintAll()
    {
        Console.WriteLine($"\nБиблиотека ({_books.Count} книг):");
        if (_books.Count == 0)
        {
            Console.WriteLine("(пусто)\n");
            return;
        }
        foreach (var b in _books)
            Console.WriteLine($"  • {b}");
        Console.WriteLine();
    }
    public void PrintList(IEnumerable<Book> list, string header)
    {
        var materialized = list.ToList();
        Console.WriteLine($"\n{header} (найдено: {materialized.Count}):");
        if (materialized.Count == 0)
        {
            Console.WriteLine("  (ничего не найдено)\n");
            return;
        }
        foreach (var b in materialized)
            Console.WriteLine($"  • {b}");
        Console.WriteLine();
    }
    // Основной метод фильтрации — принимает делегат
    public List<Book> Filter(Func<Book, bool> predicate)
    {
        return _books.Where(predicate).ToList();
    }
}
// Класс с набором фильтров (делегатов)
public static class Filters
{
    // Фильтр по автору (содержит подстроку, без учёта регистра)
    public static Func<Book, bool> ByAuthor(string author) =>
        b => b.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0;
    // Фильтр по жанру
    public static Func<Book, bool> ByGenre(string genre) =>
        b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase);
    // Фильтр по ключевому слову в названии
    public static Func<Book, bool> ByTitleKeyword(string keyword) =>
        b => b.Title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
    // Фильтр по диапазону лет
    public static Func<Book, bool> ByYearRange(int from, int to) =>
        b => b.Year >= from && b.Year <= to;
    // Фильтр: книги, изданные после указанного года
    public static Func<Book, bool> PublishedAfter(int year) =>
        b => b.Year > year;
    // Фильтр: книги, изданные до указанного года
    public static Func<Book, bool> PublishedBefore(int year) =>
        b => b.Year < year;
}
// Расширения для комбинирования делегатов
public static class PredicateExtensions
{
    public static Func<T, bool> And<T>(this Func<T, bool> first, Func<T, bool> second) =>
        x => first(x) && second(x);
    public static Func<T, bool> Or<T>(this Func<T, bool> first, Func<T, bool> second) =>
        x => first(x) || second(x);
    public static Func<T, bool> Not<T>(this Func<T, bool> predicate) =>
        x => !predicate(x);
}
class Program
{
    static void Main()
    {
        var lib = new Library();
        // Заполнение библиотеки
        lib.Add(new Book("Война и мир", "Лев Толстой", 1869, "Роман"));
        lib.Add(new Book("Анна Каренина", "Лев Толстой", 1877, "Роман"));
        lib.Add(new Book("Мастер и Маргарита", "Михаил Булгаков", 1967, "Роман"));
        lib.Add(new Book("Преступление и наказание", "Фёдор Достоевский", 1866, "Роман"));
        lib.Add(new Book("Евгений Онегин", "Александр Пушкин", 1833, "Поэма"));
        lib.Add(new Book("Капитанская дочка", "Александр Пушкин", 1836, "Повесть"));
        lib.Add(new Book("Мёртвые души", "Николай Гоголь", 1842, "Поэма"));
        lib.Add(new Book("Отцы и дети", "Иван Тургенев", 1862, "Роман"));
        lib.Add(new Book("Собачье сердце", "Михаил Булгаков", 1925, "Повесть"));
        lib.Add(new Book("Тихий Дон", "Михаил Шолохов", 1940, "Роман"));
        bool running = true;
        while (running)
        {
            Console.WriteLine("ФИЛЬТРАЦИЯ БИБЛИОТЕКИ");
            Console.WriteLine("1. Показать все книги");
            Console.WriteLine("2. Фильтр по автору");
            Console.WriteLine("3. Фильтр по жанру");
            Console.WriteLine("4. Фильтр по ключевому слову в названии");
            Console.WriteLine("5. Фильтр по диапазону лет");
            Console.WriteLine("6. Книги, изданные после указанного года");
            Console.WriteLine("7. Книги, изданные до указанного года");
            Console.WriteLine("8. Комбинированный фильтр (автор И диапазон лет)");
            Console.WriteLine("9. ИНВЕРСИЯ фильтра (НЕ автор)");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт: ");
            string choice = Console.ReadLine();
            Console.WriteLine();
            switch (choice)
            {
                case "1":
                    lib.PrintAll();
                    break;
                case "2":
                    Console.Write("Введите автора (или его часть): ");
                    string author = Console.ReadLine();
                    lib.PrintList(lib.Filter(Filters.ByAuthor(author)),
                        $"Фильтр по автору \"{author}\"");
                    break;
                case "3":
                    Console.Write("Введите жанр: ");
                    string genre = Console.ReadLine();
                    lib.PrintList(lib.Filter(Filters.ByGenre(genre)),
                        $"Фильтр по жанру \"{genre}\"");
                    break;
                case "4":
                    Console.Write("Введите ключевое слово в названии: ");
                    string keyword = Console.ReadLine();
                    lib.PrintList(lib.Filter(Filters.ByTitleKeyword(keyword)),
                        $"Книги со словом \"{keyword}\" в названии");
                    break;
                case "5":
                    Console.Write("Год от: ");
                    int from = int.Parse(Console.ReadLine());
                    Console.Write("Год до: ");
                    int to = int.Parse(Console.ReadLine());
                    lib.PrintList(lib.Filter(Filters.ByYearRange(from, to)),
                        $"Книги, изданные в {from}–{to} гг.");
                    break;
                case "6":
                    Console.Write("Год: ");
                    int after = int.Parse(Console.ReadLine());
                    lib.PrintList(lib.Filter(Filters.PublishedAfter(after)),
                        $"Книги, изданные после {after} г.");
                    break;
                case "7":
                    Console.Write("Год: ");
                    int before = int.Parse(Console.ReadLine());
                    lib.PrintList(lib.Filter(Filters.PublishedBefore(before)),
                        $"Книги, изданные до {before} г.");
                    break;
                case "8":
                    Console.Write("Автор: ");
                    string a = Console.ReadLine();
                    Console.Write("Год от: ");
                    int y1 = int.Parse(Console.ReadLine());
                    Console.Write("Год до: ");
                    int y2 = int.Parse(Console.ReadLine());
                    // Комбинирование двух делегатов через расширение And
                    Func<Book, bool> combined =
                        Filters.ByAuthor(a).And(Filters.ByYearRange(y1, y2));
                    lib.PrintList(lib.Filter(combined),
                        $"Комбинированный фильтр: автор \"{a}\" И годы {y1}–{y2}");
                    break;
                case "9":
                    Console.Write("Исключить автора: ");
                    string excl = Console.ReadLine();
                    // Инверсия делегата через расширение Not
                    Func<Book, bool> notAuthor = Filters.ByAuthor(excl).Not();
                    lib.PrintList(lib.Filter(notAuthor),
                        $"Все книги, КРОМЕ автора \"{excl}\"");
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Неверный пункт меню.\n");
                    break;
            }
        }
    }
}