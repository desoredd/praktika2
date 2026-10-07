// 1. Интерфейс "Рисунок"
public interface IRisunok
{
    void Linia(int x1, int y1, int x2, int y2);
    void Krug(int x, int y, int radius);
    void Pryamougolnik(int x, int y, int shirina, int visota);
    void Pokazat();   // показать всё, что нарисовано
    void Ochistit();  // очистить холст
}
// 2. Класс "Холст" — реализует интерфейс IRisunok
public class Holst : IRisunok
{
    // Журнал всех нарисованных фигур
    private List<string> figury = new List<string>();
    public void Linia(int x1, int y1, int x2, int y2)
    {
        figury.Add($"Линия: ({x1},{y1}) → ({x2},{y2})");
        Console.WriteLine($"Нарисована линия от ({x1},{y1}) до ({x2},{y2})");
    }
    public void Krug(int x, int y, int radius)
    {
        if (radius <= 0)
        {
            Console.WriteLine("Ошибка: радиус должен быть положительным.");
            return;
        }
        figury.Add($"Круг: центр ({x},{y}), радиус {radius}");
        Console.WriteLine($"Нарисован круг с центром ({x},{y}) и радиусом {radius}");
    }
    public void Pryamougolnik(int x, int y, int shirina, int visota)
    {
        if (shirina <= 0 || visota <= 0)
        {
            Console.WriteLine("Ошибка: ширина и высота должны быть положительными.");
            return;
        }
        figury.Add($"Прямоугольник: ({x},{y}), {shirina}x{visota}");
        Console.WriteLine($"Нарисован прямоугольник в ({x},{y}) размером {shirina}x{visota}");
    }
    public void Pokazat()
    {
        if (figury.Count == 0)
        {
            Console.WriteLine("Холст пуст.");
            return;
        }
        Console.WriteLine("\n=== Содержимое холста ===");
        for (int i = 0; i < figury.Count; i++)
            Console.WriteLine($"{i + 1}. {figury[i]}");
        Console.WriteLine($"Всего фигур: {figury.Count}");
    }
    public void Ochistit()
    {
        figury.Clear();
        Console.WriteLine("Холст очищен.");
    }
}
// 3. Основная программа
class Program
{
    static void Main()
    {
        // Используем интерфейсную переменную — это полиморфизм
        IRisunok holst = new Holst();
        bool run = true;
        while (run)
        {
            Console.WriteLine("Графический редактор");
            Console.WriteLine("1 - Нарисовать линию");
            Console.WriteLine("2 - Нарисовать круг");
            Console.WriteLine("3 - Нарисовать прямоугольник");
            Console.WriteLine("4 - Показать холст");
            Console.WriteLine("5 - Очистить холст");
            Console.WriteLine("0 - Выход");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Введите x1: ");
                    int x1 = ReadInt();
                    Console.Write("Введите y1: ");
                    int y1 = ReadInt();
                    Console.Write("Введите x2: ");
                    int x2 = ReadInt();
                    Console.Write("Введите y2: ");
                    int y2 = ReadInt();
                    holst.Linia(x1, y1, x2, y2);
                    break;
                case "2":
                    Console.Write("Введите x центра: ");
                    int cx = ReadInt();
                    Console.Write("Введите y центра: ");
                    int cy = ReadInt();
                    Console.Write("Введите радиус: ");
                    int r = ReadInt();
                    holst.Krug(cx, cy, r);
                    break;
                case "3":
                    Console.Write("Введите x: ");
                    int px = ReadInt();
                    Console.Write("Введите y: ");
                    int py = ReadInt();
                    Console.Write("Введите ширину: ");
                    int w = ReadInt();
                    Console.Write("Введите высоту: ");
                    int h = ReadInt();
                    holst.Pryamougolnik(px, py, w, h);
                    break;
                case "4":
                    holst.Pokazat();
                    break;
                case "5":
                    holst.Ochistit();
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
    // Вспомогательный метод для безопасного ввода числа
    static int ReadInt()
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int value))
                return value;
            Console.Write("Ошибка. Введите целое число: ");
        }
    }
}