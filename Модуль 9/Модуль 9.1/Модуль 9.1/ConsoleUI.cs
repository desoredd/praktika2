using StudentManager.Models;
using StudentManager.Services;
namespace StudentManager.UI;
public class ConsoleUI
{
    private readonly StudentService _service;
    public ConsoleUI(StudentService service)
    {
        _service = service;
    }
    // Вспомогательные методы
    private int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value)) return value;
            Console.WriteLine("Ошибка: введите целое число.");
        }
    }
    private double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine(), out var value)) return value;
            Console.WriteLine("Ошибка: введите число.");
        }
    }
    private string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var value = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(value)) return value;
            Console.WriteLine("Ошибка: поле не может быть пустым.");
        }
    }
    private int ReadAge()
    {
        while (true)
        {
            var age = ReadInt("Возраст: ");
            if (age is >= 14 and <= 100) return age;
            Console.WriteLine("Ошибка: возраст должен быть от 14 до 100.");
        }
    }
    private double ReadGrade()
    {
        while (true)
        {
            var grade = ReadDouble("Средний балл (0-10): ");
            if (grade is >= 0 and <= 10) return grade;
            Console.WriteLine("Ошибка: средний балл должен быть от 0 до 10.");
        }
    }
    private static void PrintStudents(IEnumerable<Student> students)
    {
        var list = students.ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("\n[Список пуст]\n");
            return;
        }
        Console.WriteLine(new string('-', 90));
        foreach (var s in list) Console.WriteLine(s);
        Console.WriteLine(new string('-', 90));
        Console.WriteLine($"Всего студентов: {list.Count}\n");
    }
    // Действия
    private void ShowAll()
    {
        Console.WriteLine("\nСписок студентов");
        PrintStudents(_service.GetAllStudents());
    }
    private void AddStudent()
    {
        Console.WriteLine("\nДобавление студента");
        var firstName = ReadNonEmpty("Имя: ");
        var lastName = ReadNonEmpty("Фамилия: ");
        var age = ReadAge();
        var group = ReadNonEmpty("Группа: ");
        var grade = ReadGrade();
        var student = _service.AddStudent(firstName, lastName, age, group, grade);
        Console.WriteLine($"\nСтудент добавлен (ID: {student.Id}).\n");
    }
    private void EditStudent()
    {
        Console.WriteLine("\nРедактирование студента");
        var id = ReadInt("ID студента: ");
        var student = _service.GetStudent(id);
        if (student == null)
        {
            Console.WriteLine("Студент с таким ID не найден.\n");
            return;
        }
        Console.WriteLine($"Текущие данные:\n{student}\n");
        Console.WriteLine("Оставьте поле пустым, чтобы не менять значение.");
        Console.Write($"Имя [{student.FirstName}]: ");
        var firstName = Console.ReadLine()?.Trim();
        Console.Write($"Фамилия [{student.LastName}]: ");
        var lastName = Console.ReadLine()?.Trim();
        Console.Write($"Возраст [{student.Age}]: ");
        var ageInput = Console.ReadLine()?.Trim();
        int? age = int.TryParse(ageInput, out var a) ? a : null;
        Console.Write($"Группа [{student.Group}]: ");
        var group = Console.ReadLine()?.Trim();
        Console.Write($"Средний балл [{student.AverageGrade}]: ");
        var gradeInput = Console.ReadLine()?.Trim();
        double? grade = double.TryParse(gradeInput, out var g) ? g : null;
        var success = _service.UpdateStudent(id, updated =>
        {
            if (!string.IsNullOrEmpty(firstName)) updated.FirstName = firstName;
            if (!string.IsNullOrEmpty(lastName)) updated.LastName = lastName;
            if (age.HasValue) updated.Age = age.Value;
            if (!string.IsNullOrEmpty(group)) updated.Group = group;
            if (grade.HasValue) updated.AverageGrade = grade.Value;
        });
        Console.WriteLine(success ? "Данные обновлены.\n" : "Ошибка обновления.\n");
    }
    private void DeleteStudent()
    {
        Console.WriteLine("\nУдаление студента");
        var id = ReadInt("ID студента для удаления: ");
        Console.WriteLine(_service.DeleteStudent(id)
            ? "Студент удалён.\n"
            : "Студент с таким ID не найден.\n");
    }
    private void SearchStudents()
    {
        Console.WriteLine("\nПоиск студентов");
        Console.Write("Введите имя/фамилию/группу (Enter — показать всех): ");
        var query = Console.ReadLine() ?? string.Empty;
        PrintStudents(_service.Search(query));
    }
    private void SortStudents()
    {
        Console.WriteLine("\nСортировка студентов");
        foreach (var (sortKey, sortName) in StudentService.SortCriteria)
            Console.WriteLine($"{sortKey}. {sortName}");
        Console.Write("Выберите критерий: ");
        var key = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!StudentService.SortCriteria.ContainsKey(key))
        {
            Console.WriteLine("Неверный критерий.\n");
            return;
        }
        Console.Write("По убыванию? (y/n): ");
        var reverse = Console.ReadLine()?.Trim().ToLower() == "y";
        try
        {
            PrintStudents(_service.SortStudents(key, reverse));
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\n");
        }
    }
    // Главный цикл
    private static readonly Dictionary<string, (string Label, Action<ConsoleUI> Action)> Menu = new()
    {
        ["1"] = ("Показать всех студентов", ui => ui.ShowAll()),
        ["2"] = ("Добавить студента", ui => ui.AddStudent()),
        ["3"] = ("Редактировать студента", ui => ui.EditStudent()),
        ["4"] = ("Удалить студента", ui => ui.DeleteStudent()),
        ["5"] = ("Поиск студентов", ui => ui.SearchStudents()),
        ["6"] = ("Сортировка студентов", ui => ui.SortStudents()),
        ["0"] = ("Выход", _ => { }
        )
    };
    public void Run()
    {
        Console.WriteLine("   СИСТЕМА УПРАВЛЕНИЯ СПИСКОМ СТУДЕНТОВ");
        while (true)
        {
            Console.WriteLine("\nМеню");
            foreach (var (key, item) in Menu)
                Console.WriteLine($"{key}. {item.Label}");
            Console.Write("Выберите действие: ");
            var choice = Console.ReadLine()?.Trim() ?? string.Empty;
            if (choice == "0")
            {
                Console.WriteLine("Выход. Данные сохранены.");
                break;
            }
            if (!Menu.TryGetValue(choice, out var menuItem))
            {
                Console.WriteLine("Неверный выбор.\n");
                continue;
            }
            menuItem.Action(this);
        }
    }
}