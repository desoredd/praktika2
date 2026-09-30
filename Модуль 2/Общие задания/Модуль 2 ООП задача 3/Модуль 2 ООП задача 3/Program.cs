// Класс Author — информация об авторе
public class Author
{
    public string Name { get; set; }
    public int BirthYear { get; set; }
    public Author(string name, int birthYear)
    {
        Name = name;
        BirthYear = birthYear;
    }
    // Метод для вывода информации об авторе
    public void PrintInfo()
    {
        Console.WriteLine($"Автор: {Name} (род. {BirthYear})");
    }
    public override string ToString()
    {
        return $"{Name} (род. {BirthYear})";
    }
}
// Класс Book — информация о книге, содержит Author
public class Book
{
    public string Title { get; set; }
    public int Year { get; set; }
    public Author Author { get; set; }   // композиция: книга содержит автора
    public Book(string title, int year, Author author)
    {
        Title = title;
        Year = year;
        Author = author;
    }
    // Метод для вывода информации о книге
    public void PrintInfo()
    {
        Console.WriteLine($"Книга: {Title} ({Year} г.)");
        Console.WriteLine($"Автор: {Author.Name} (род. {Author.BirthYear})");
        Console.WriteLine();
    }
    public override string ToString()
    {
        return $"{Title} ({Year}) — {Author}";
    }
}
class Program
{
    static void Main()
    {
        // Создаём авторов
        Author pushkin = new Author("Александр Пушкин", 1799);
        Author tolstoy = new Author("Лев Толстой", 1828);
        Author bulgakov = new Author("Михаил Булгаков", 1891);
        // Создаём книги, связывая их с авторами (композиция)
        Book[] books =
        {
            new Book("Евгений Онегин", 1833, pushkin),
            new Book("Война и мир", 1869, tolstoy),
            new Book("Мастер и Маргарита", 1967, bulgakov),
            new Book("Анна Каренина", 1877, tolstoy),
            new Book("Капитанская дочка", 1836, pushkin)
        };
        // Вывод информации о книгах
        Console.WriteLine("Список книг");
        Console.WriteLine();
        foreach (Book book in books)
        {
            book.PrintInfo();
        }
        // Демонстрация: две книги одного автора ссылаются на один объект Author
        Console.WriteLine("Проверка композиции");
        Console.WriteLine("books[1].Author == books[3].Author? " +$"{ReferenceEquals(books[1].Author, books[3].Author)}");
    }
}