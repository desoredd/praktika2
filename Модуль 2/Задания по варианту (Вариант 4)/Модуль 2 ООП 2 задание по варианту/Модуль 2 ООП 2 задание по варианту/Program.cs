using System.Collections;
// Класс "Книга"
public class Book : IEquatable<Book>
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
    public bool Equals(Book? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Title == other.Title
            && Author == other.Author
            && Year == other.Year;
    }
    public override bool Equals(object? obj) => Equals(obj as Book);
    public override int GetHashCode() => HashCode.Combine(Title, Author, Year);
    public override string ToString()
        => $"\"{Title}\" — {Author} ({Year} г., {Genre})";
}
// Класс "Домашняя библиотека"
public class HomeLibrary : IEnumerable<Book>
{
    private readonly List<Book> books = new();
    // Свойства
    public int Count => books.Count;
    // Индексатор
    public Book this[int index]
    {
        get
        {
            if (index < 0 || index >= books.Count)
                throw new IndexOutOfRangeException("Неверный индекс книги.");
            return books[index];
        }
    }
    // Добавление
    public void Add(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));
        if (books.Contains(book))
        {
            Console.WriteLine($"Книга {book} уже есть в библиотеке.");
            return;
        }
        books.Add(book);
        Console.WriteLine($"Добавлена книга: {book}");
    }
    public void AddRange(IEnumerable<Book> list)
    {
        foreach (var b in list) Add(b);
    }
    // Удаление
    public bool Remove(Book book) => books.Remove(book);
    public bool RemoveByTitle(string title)
    {
        var book = books.FirstOrDefault(b =>
            string.Equals(b.Title, title, StringComparison.OrdinalIgnoreCase));
        if (book == null) return false;
        return books.Remove(book);
    }
    public int RemoveAllByAuthor(string author)
    {
        return books.RemoveAll(b =>
            string.Equals(b.Author, author, StringComparison.OrdinalIgnoreCase));
    }
    // Поиск
    public List<Book> Find(Func<Book, bool> predicate)
    {
        return books.Where(predicate).ToList();
    }
    public List<Book> FindByAuthor(string author)
        => books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
    public List<Book> FindByYear(int year)
        => books.Where(b => b.Year == year).ToList();
    public List<Book> FindByYearRange(int from, int to)
        => books.Where(b => b.Year >= from && b.Year <= to)
                .OrderBy(b => b.Year)
                .ToList();
    public List<Book> FindByTitle(string title)
        => books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
    public List<Book> FindByGenre(string genre)
        => books.Where(b => string.Equals(b.Genre, genre, StringComparison.OrdinalIgnoreCase)).ToList();
    // Сортировка по ключу
    public void SortBy<TKey>(Func<Book, TKey> keySelector, bool descending = false)
    {
        var sorted = descending
            ? books.OrderByDescending(keySelector).ToList()
            : books.OrderBy(keySelector).ToList();
        books.Clear();
        books.AddRange(sorted);
    }
    public void SortByTitle(bool descending = false) => SortBy(b => b.Title, descending);
    public void SortByAuthor(bool descending = false) => SortBy(b => b.Author, descending);
    public void SortByYear(bool descending = false) => SortBy(b => b.Year, descending);
    public void SortByGenre(bool descending = false) => SortBy(b => b.Genre, descending);
    // Сортировка по нескольким полям
    public void SortByAuthorThenYear(bool descending = false)
    {
        var sorted = descending
            ? books.OrderByDescending(b => b.Author).ThenByDescending(b => b.Year).ToList()
            : books.OrderBy(b => b.Author).ThenBy(b => b.Year).ToList();
        books.Clear();
        books.AddRange(sorted);
    }
    // Статистика
    public void PrintAll()
    {
        Console.WriteLine($"\nБиблиотека ({books.Count} книг)");
        if (books.Count == 0)
        {
            Console.WriteLine("(пусто)\n");
            return;
        }
        for (int i = 0; i < books.Count; i++)
            Console.WriteLine($"{i + 1,3}. {books[i]}");
        Console.WriteLine();
    }
    public Dictionary<string, int> CountByAuthor()
        => books.GroupBy(b => b.Author)
                .ToDictionary(g => g.Key, g => g.Count());
    public Book? OldestBook()
        => books.Count == 0 ? null : books.OrderBy(b => b.Year).First();
    public Book? NewestBook()
        => books.Count == 0 ? null : books.OrderByDescending(b => b.Year).First();
    public double AverageYear()
        => books.Count == 0 ? 0 : books.Average(b => b.Year);
    // Реализация IEnumerable<Book>
    public IEnumerator<Book> GetEnumerator() => books.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var lib = new HomeLibrary();
        // Добавление книг
        Console.WriteLine("Добавление книг");
        lib.Add(new Book("Война и мир", "Лев Толстой", 1869, "Роман"));
        lib.Add(new Book("Анна Каренина", "Лев Толстой", 1877, "Роман"));
        lib.Add(new Book("Мастер и Маргарита", "Михаил Булгаков", 1967, "Роман"));
        lib.Add(new Book("Преступление и наказание", "Фёдор Достоевский", 1866, "Роман"));
        lib.Add(new Book("Евгений Онегин", "Александр Пушкин", 1833, "Поэма"));
        lib.Add(new Book("Капитанская дочка", "Александр Пушкин", 1836, "Повесть"));
        lib.Add(new Book("Мёртвые души", "Николай Гоголь", 1842, "Поэма"));
        lib.Add(new Book("Отцы и дети", "Иван Тургенев", 1862, "Роман"));
        lib.PrintAll();
        // Поиск по автору
        Console.WriteLine("Поиск книг Льва Толстого");
        foreach (var b in lib.FindByAuthor("Толстой"))
            Console.WriteLine("  " + b);
        Console.WriteLine();
        // Поиск по году
        Console.WriteLine("Книги 1833 года");
        foreach (var b in lib.FindByYear(1833))
            Console.WriteLine("  " + b);
        Console.WriteLine();
        // Поиск по диапазону лет
        Console.WriteLine("Книги 1860–1880 гг.");
        foreach (var b in lib.FindByYearRange(1860, 1880))
            Console.WriteLine("  " + b);
        Console.WriteLine();
        // Поиск по жанру
        Console.WriteLine("Жанр: Роман");
        foreach (var b in lib.FindByGenre("Роман"))
            Console.WriteLine("  " + b);
        Console.WriteLine();
        Console.WriteLine("Сортировка по году");
        lib.SortByYear();
        lib.PrintAll();
        Console.WriteLine("Сортировка по автору");
        lib.SortByAuthor(descending: true);
        lib.PrintAll();
        Console.WriteLine("Сортировка по автору, затем по году");
        lib.SortByAuthorThenYear();
        lib.PrintAll();
        Console.WriteLine("Удаление");
        bool removedByTitle = lib.RemoveByTitle("Мёртвые души");
        Console.WriteLine($"Удалена «Мёртвые души»: {removedByTitle}");
        int removedByAuthor = lib.RemoveAllByAuthor("Пушкин");
        Console.WriteLine($"Удалено книг Пушкина: {removedByAuthor}");
        lib.PrintAll();
        Console.WriteLine("Статистика");
        Console.WriteLine($"Всего книг: {lib.Count}");
        var oldest = lib.OldestBook();
        var newest = lib.NewestBook();
        Console.WriteLine($"Самая старая: {(oldest is null ? "нет книг" : oldest.ToString())}");
        Console.WriteLine($"Самая новая:  {(newest is null ? "нет книг" : newest.ToString())}");
        Console.WriteLine($"Средний год:  {lib.AverageYear():F0}");
        Console.WriteLine("Книг по авторам:");
        foreach (var pair in lib.CountByAuthor())
            Console.WriteLine($"  {pair.Key}: {pair.Value}");
        Console.WriteLine();
        // Перебор через foreach (IEnumerable)
        Console.WriteLine("Перебор через foreach");
        foreach (var book in lib)
            Console.WriteLine("  " + book);
        Console.WriteLine();
        // Поиск через предикат
        Console.WriteLine("Предикат: книги после 1865 и жанр «Роман»");
        foreach (var b in lib.Find(x => x.Year > 1865 && x.Genre == "Роман"))
            Console.WriteLine("  " + b);
    }
}