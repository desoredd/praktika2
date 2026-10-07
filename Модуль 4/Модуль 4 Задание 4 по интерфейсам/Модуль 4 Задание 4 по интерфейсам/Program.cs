// 1. Интерфейс "Книга"
public interface Book
{
    string Nazvanie { get; }
    bool Dostupna();
    void Vydat();                  
    string Info();           
}
// 2. Учебник — явно реализует Book
public class Uchebnik : Book
{
    public string Nazvanie { get; set; }
    private string predmet;
    private bool vydana;
    public Uchebnik(string nazvanie, string predmet)
    {
        Nazvanie = nazvanie;
        this.predmet = predmet;
        vydana = false;    
    }
    public bool Dostupna() => !vydana;
    public void Vydat()
    {
        if (vydana)
        {
            Console.WriteLine($"Книга «{Nazvanie}» уже выдана.");
            return;
        }
        vydana = true;
        Console.WriteLine($"Учебник «{Nazvanie}» выдан.");
    }
    public string Info() => $"Учебник по предмету: {predmet}";
}
// 3. Роман — явно реализует IBook
public class Roman : Book
{
    public string Nazvanie { get; set; }
    private string avtor;
    private bool vydana;
    public Roman(string nazvanie, string avtor)
    {
        Nazvanie = nazvanie;
        this.avtor = avtor;
        vydana = false;
    }
    public bool Dostupna() => !vydana;
    public void Vydat()
    {
        if (vydana)
        {
            Console.WriteLine($"Книга «{Nazvanie}» уже выдана.");
            return;
        }
        vydana = true;
        Console.WriteLine($"Роман «{Nazvanie}» выдан.");
    }
    public string Info() => $"Роман автора: {avtor}";
}
// 4. Журнал — явно реализует IBook
public class Zhurnal : Book
{
    public string Nazvanie { get; set; }
    private int nomer;
    private bool vydana;
    public Zhurnal(string nazvanie, int nomer)
    {
        Nazvanie = nazvanie;
        this.nomer = nomer;
        vydana = false;
    }
    public bool Dostupna() => !vydana;
    public void Vydat()
    {
        if (vydana)
        {
            Console.WriteLine($"Журнал «{Nazvanie}» уже выдан.");
            return;
        }
        vydana = true;
        Console.WriteLine($"Журнал «{Nazvanie}» выдан.");
    }
    public string Info() => $"Журнал, выпуск №{nomer}";
}
// 5. Основная программа
class Program
{
    static void Main()
    {
        List<Book> biblioteka = new List<Book>
        {
            new Uchebnik("Алгебра 9 класс", "Математика"),
            new Roman("Война и мир", "Л. Н. Толстой"),
            new Zhurnal("Наука и жизнь", 5)
        };
        bool run = true;
        while (run)
        {
            Console.WriteLine("Библиотека");
            Console.WriteLine("1 - Показать все книги");
            Console.WriteLine("2 - Проверить доступность");
            Console.WriteLine("3 - Выдать книгу");
            Console.WriteLine("4 - Добавить книгу");
            Console.WriteLine("0 - Выход");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    for (int i = 0; i < biblioteka.Count; i++)
                        Console.WriteLine($"{i + 1}. {biblioteka[i].Nazvanie} — {biblioteka[i].Info()}");
                    break;
                case "2":
                    for (int i = 0; i < biblioteka.Count; i++)
                    {
                        string status = biblioteka[i].Dostupna() ? "доступна" : "выдана";
                        Console.WriteLine($"{i + 1}. {biblioteka[i].Nazvanie} — {status}");
                    }
                    break;
                case "3":
                    Console.Write("Введите номер книги: ");
                    if (!int.TryParse(Console.ReadLine(), out int num) ||
                        num < 1 || num > biblioteka.Count)
                    {
                        Console.WriteLine("Неверный номер.");
                        break;
                    }
                    biblioteka[num - 1].Vydat();
                    break;
                case "4":
                    Console.Write("Введите тип (1 - учебник, 2 - роман, 3 - журнал): ");
                    if (!int.TryParse(Console.ReadLine(), out int tip))
                    {
                        Console.WriteLine("Ошибка: нужно число.");
                        break;
                    }
                    Console.Write("Введите название: ");
                    string nazvanie = Console.ReadLine();
                    Book newBook = null;
                    if (tip == 1)
                    {
                        Console.Write("Введите предмет: ");
                        string predmet = Console.ReadLine();
                        newBook = new Uchebnik(nazvanie, predmet);
                    }
                    else if (tip == 2)
                    {
                        Console.Write("Введите автора: ");
                        string avtor = Console.ReadLine();
                        newBook = new Roman(nazvanie, avtor);
                    }
                    else if (tip == 3)
                    {
                        Console.Write("Введите номер выпуска: ");
                        if (!int.TryParse(Console.ReadLine(), out int nomer))
                        {
                            Console.WriteLine("Ошибка: номер должен быть числом.");
                            break;
                        }
                        newBook = new Zhurnal(nazvanie, nomer);
                    }
                    else
                    {
                        Console.WriteLine("Неверный тип книги.");
                        break;
                    }
                    biblioteka.Add(newBook);
                    Console.WriteLine("Книга добавлена в библиотеку!");
                    break;
                case "0":
                    run = false;
                    break;
                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }
}