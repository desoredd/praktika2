// Интерфейс IDrawable — контракт для всех "рисуемых" объектов
public interface IDrawable
{
    void Draw();
}
// Класс Circle реализует IDrawable
public class Circle : IDrawable
{
    public double Radius { get; set; }
    public Circle(double radius)
    {
        Radius = radius;
    }
    public void Draw()
    {
        Console.WriteLine($"[Круг] Радиус = {Radius}, " + $"площадь = {Math.PI * Radius * Radius:F2}, " + $"длина окружности = {2 * Math.PI * Radius:F2}");
    }
}
// Класс Rectangle реализует IDrawable
public class Rectangle : IDrawable
{
    public double Width { get; set; }
    public double Height { get; set; }
    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }
    public void Draw()
    {
        Console.WriteLine($"[Прямоугольник] {Width} x {Height}, " + $"площадь = {Width * Height:F2}, " + $"периметр = {2 * (Width + Height):F2}");
    }
}
// Класс Triangle реализует IDrawable
public class Triangle : IDrawable
{
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }
    public Triangle(double a, double b, double c)
    {
        A = a;
        B = b;
        C = c;
    }
    public void Draw()
    {
        // Полупериметр для формулы Герона
        double p = (A + B + C) / 2;
        double area = Math.Sqrt(p * (p - A) * (p - B) * (p - C));
        Console.WriteLine($"[Треугольник] стороны {A}, {B}, {C}, " + $"площадь = {area:F2}, " + $"периметр = {A + B + C:F2}");
    }
}
class Program
{
    static void Main()
    {
        // Массив объектов, реализующих интерфейс IDrawable
        IDrawable[] shapes =
        {
            new Circle(5),
            new Rectangle(4, 6),
            new Triangle(3, 4, 5),
            new Circle(1.5),
            new Rectangle(10, 2),
            new Triangle(6, 8, 10)
        };
        // Полиморфный вызов Draw() для каждого объекта
        foreach (IDrawable shape in shapes)
        {
            shape.Draw();
        }
    }
}