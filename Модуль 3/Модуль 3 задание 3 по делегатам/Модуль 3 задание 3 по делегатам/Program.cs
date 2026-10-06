// Объявление делегата для обработки задачи
public delegate void TaskHandler(string taskName);
// Класс задачи
public class TaskItem
{
    public string Name { get; set; }
    public TaskHandler Handler { get; set; }
    public TaskItem(string name, TaskHandler handler)
    {
        Name = name;
        Handler = handler;
    }
    public void Execute()
    {
        Console.Write($"[Задача: {Name}] ");
        Handler?.Invoke(Name);
    }
}
// Класс менеджера задач
public class TaskManager
{
    private readonly List<TaskItem> _tasks = new List<TaskItem>();
    public void AddTask(TaskItem task)
    {
        _tasks.Add(task);
        Console.WriteLine($"Задача \"{task.Name}\" добавлена.");
    }
    public void ExecuteAll()
    {
        if (_tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }
        Console.WriteLine("\nВыполнение задач");
        foreach (var task in _tasks)
        {
            task.Execute();
        }
        Console.WriteLine("Все задачи выполнены\n");
    }
    public int Count => _tasks.Count;
}
// Класс с методами-обработчиками (делегаты)
public static class Handlers
{
    public static void SendNotification(string taskName)
    {
        Console.WriteLine($"Отправка уведомления по задаче \"{taskName}\" на email пользователя.");
    }
    public static void WriteToLog(string taskName)
    {
        Console.WriteLine($"Запись в журнал: задача \"{taskName}\" обработана в {DateTime.Now:HH:mm:ss}.");
    }
    public static void SendSms(string taskName)
    {
        Console.WriteLine($"Отправка SMS: задача \"{taskName}\" выполнена.");
    }
    public static void PrintToConsole(string taskName)
    {
        Console.WriteLine($"Вывод на консоль: выполнена задача \"{taskName}\".");
    }
}
class Program
{
    static void Main()
    {
        var manager = new TaskManager();
        // Массив доступных обработчиков
        TaskHandler[] handlers = new TaskHandler[]
        {
                Handlers.SendNotification,
                Handlers.WriteToLog,
                Handlers.SendSms,
                Handlers.PrintToConsole
        };
        string[] handlerNames = new string[]
        {
                "Отправка уведомления",
                "Запись в журнал",
                "Отправка SMS",
                "Вывод в консоль"
        };
        bool running = true;
        while (running)
        {
            Console.WriteLine("МЕНЕДЖЕР ЗАДАЧ");
            Console.WriteLine("1. Добавить задачу");
            Console.WriteLine("2. Выполнить все задачи");
            Console.WriteLine("3. Показать количество задач");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddTask(manager, handlers, handlerNames);
                    break;
                case "2":
                    manager.ExecuteAll();
                    break;
                case "3":
                    Console.WriteLine($"Всего задач: {manager.Count}\n");
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
    static void AddTask(TaskManager manager, TaskHandler[] handlers, string[] handlerNames)
    {
        Console.Write("Введите название задачи: ");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Название не может быть пустым.\n");
            return;
        }
        Console.WriteLine("Выберите делегата для выполнения задачи:");
        for (int i = 0; i < handlers.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {handlerNames[i]}");
        }
        Console.Write("Ваш выбор: ");
        if (int.TryParse(Console.ReadLine(), out int idx) &&
            idx >= 1 && idx <= handlers.Length)
        {
            var task = new TaskItem(name, handlers[idx - 1]);
            manager.AddTask(task);
        }
        else
        {
            Console.WriteLine("Неверный выбор делегата.\n");
        }
        Console.WriteLine();
    }
}