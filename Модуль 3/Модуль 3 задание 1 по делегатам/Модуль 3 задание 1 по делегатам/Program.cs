// Базовый класс "Фигура"
public abstract class Figure
{
    public string Name { get; protected set; }

    // Абстрактный метод для вычисления площади
    public abstract double GetArea();

    public override string ToString()
    {
        return $"{Name}: площадь = {GetArea():F2}";
    }
}
// Круг
public class Circle : Figure
{
    public double Radius { get; set; }
    public Circle(double radius)
    {
        Name = "Круг";
        Radius = radius;
    }
    public override double GetArea()
    {
        return Math.PI * Math.Pow(Radius, 2);
    }
}
// Прямоугольник
public class Rectangle : Figure
{
    public double Width { get; set; }
    public double Height { get; set; }
    public Rectangle(double width, double height)
    {
        Name = "Прямоугольник";
        Width = width;
        Height = height;
    }
    public override double GetArea()
    {
        return Width * Height;
    }
}
// Треугольник
public class Triangle : Figure
{
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }
    public Triangle(double a, double b, double c)
    {
        // Проверка существования треугольника
        if (a + b <= c || a + c <= b || b + c <= a)
            throw new ArgumentException("Треугольник с такими сторонами не существует");
        Name = "Треугольник";
        A = a;
        B = b;
        C = c;
    }
    public override double GetArea()
    {
        double p = (A + B + C) / 2; // полупериметр
        return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
    }
}
class Program
{
    // Объявление делегата для метода вычисления площади
    public delegate double AreaCalculator();
    static void Main(string[] args)
    {
        // Создаём фигуры
        Figure[] figures =
        {
                new Circle(5),
                new Rectangle(4, 6),
                new Triangle(3, 4, 5)
            };
        Console.WriteLine("Вычисление площадей через делегат\n");
        foreach (var figure in figures)
        {
            // Привязываем делегат к методу GetArea конкретного объекта
            AreaCalculator calculator = figure.GetArea;
            // Динамический вызов через делегат
            double area = calculator();
            Console.WriteLine($"{figure.Name}: площадь = {area:F4}");
        }
        Console.WriteLine("\nДемонстрация динамического изменения делегата\n");
        // Динамическое переключение делегата между разными фигурами
        AreaCalculator dynamicCalc;
        Figure current;
        current = new Rectangle(10, 10);
        dynamicCalc = current.GetArea;
        Console.WriteLine($"Квадрат 10x10: площадь = {dynamicCalc():F4}");
        current = new Circle(3);
        dynamicCalc = current.GetArea;
        Console.WriteLine($"Круг радиусом 3: площадь = {dynamicCalc():F4}");
        current = new Triangle(6, 8, 10);
        dynamicCalc = current.GetArea;
        Console.WriteLine($"Треугольник 6-8-10: площадь = {dynamicCalc():F4}");
    }
}