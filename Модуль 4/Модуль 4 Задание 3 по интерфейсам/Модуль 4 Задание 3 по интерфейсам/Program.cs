// 1. Интерфейс "Студент"
public interface IStudent
{
    double SredniyBall();          // средний балл
    string InformaciyaOKurse();    // информация о курсе
    string Name { get; }           // имя студента
}
// 2. Студент 1 курса — реализует IStudent
public class Student1Kurs : IStudent
{
    public string Name { get; set; }
    private List<int> ocenki;
    public Student1Kurs(string name, List<int> ocenki)
    {
        Name = name;
        this.ocenki = ocenki;
    }
    public double SredniyBall()
    {
        if (ocenki.Count == 0) return 0;
        return ocenki.Average();
    }
    public string InformaciyaOKurse()
    {
        return $"{Name} учится на 1 курсе. Предметы: Математика, Физика, Информатика.";
    }
}
// 3. Студент 2 курса — реализует IStudent
public class Student2Kurs : IStudent
{
    public string Name { get; set; }
    private List<int> ocenki;
    public Student2Kurs(string name, List<int> ocenki)
    {
        Name = name;
        this.ocenki = ocenki;
    }
    public double SredniyBall()
    {
        if (ocenki.Count == 0) return 0;
        return ocenki.Average();
    }
    public string InformaciyaOKurse()
    {
        return $"{Name} учится на 2 курсе. Предметы: Алгоритмы, Базы данных, ООП.";
    }
}
// 4. Студент 3 курса — реализует IStudent
public class Student3Kurs : IStudent
{
    public string Name { get; set; }
    private List<int> ocenki;
    public Student3Kurs(string name, List<int> ocenki)
    {
        Name = name;
        this.ocenki = ocenki;
    }
    public double SredniyBall()
    {
        if (ocenki.Count == 0) return 0;
        return ocenki.Average();
    }
    public string InformaciyaOKurse()
    {
        return $"{Name} учится на 3 курсе. Предметы: Проектирование, Тестирование, Диплом.";
    }
}
// 5. Основная программа
class Program
{
    static void Main()
    {
        List<IStudent> students = new List<IStudent>
        {
            new Student1Kurs("Иван", new List<int> { 8, 5, 7, 8 }),
            new Student2Kurs("Мария", new List<int> { 9, 9, 9, 9 }),
            new Student3Kurs("Петр", new List<int> { 3, 4, 4, 3 })
        };
        bool run = true;
        while (run)
        {
            Console.WriteLine("Меню");
            Console.WriteLine("1 - Показать всех студентов");
            Console.WriteLine("2 - Показать средний балл всех студентов");
            Console.WriteLine("3 - Информация о курсах");
            Console.WriteLine("4 - Добавить студента");
            Console.WriteLine("0 - Выход");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    foreach (var s in students)
                        Console.WriteLine($"Студент: {s.Name}");
                    break;
                case "2":
                    foreach (var s in students)
                        Console.WriteLine($"{s.Name}: средний балл = {s.SredniyBall():F2}");
                    break;
                case "3":
                    foreach (var s in students)
                        Console.WriteLine(s.InformaciyaOKurse());
                    break;
                case "4":
                    Console.Write("Введите имя: ");
                    string name = Console.ReadLine();
                    Console.Write("Введите курс (1, 2 или 3): ");
                    if (!int.TryParse(Console.ReadLine(), out int kurs))
                    {
                        Console.WriteLine("Ошибка: курс должен быть числом.");
                        break;
                    }
                    Console.Write("Введите оценки через запятую (например, 4,5,3): ");
                    string[] parts = Console.ReadLine().Split(',');
                    List<int> ocenki = new List<int>();
                    bool ok = true;
                    foreach (var p in parts)
                    {
                        if (int.TryParse(p.Trim(), out int o))
                            ocenki.Add(o);
                        else
                        {
                            Console.WriteLine($"Ошибка: '{p}' — не число.");
                            ok = false;
                            break;
                        }
                    }
                    if (!ok) break;
                    IStudent newStudent = kurs switch
                    {
                        1 => new Student1Kurs(name, ocenki),
                        2 => new Student2Kurs(name, ocenki),
                        3 => new Student3Kurs(name, ocenki),
                        _ => null
                    };
                    if (newStudent == null)
                        Console.WriteLine("Неверный курс. Допустимо: 1, 2, 3.");
                    else
                    {
                        students.Add(newStudent);
                        Console.WriteLine("Студент добавлен!");
                    }
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