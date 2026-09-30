// Базовый класс Shape
public class Shape
{
    // Виртуальные методы, которые будут переопределены в производных классах
    public virtual double Area()
    {
        return 0;
    }
    public virtual double Perimeter()
    {
        return 0;
    }
    // Виртуальный метод для вывода информации о фигуре
    public virtual void PrintInfo()
    {
        Console.WriteLine($"Фигура: {GetType().Name}");
        Console.WriteLine($"Площадь: {Area():F2}");
        Console.WriteLine($"Периметр: {Perimeter():F2}");
        Console.WriteLine();
    }
}
// Производный класс Circle
public class Circle : Shape
{
    public double Radius { get; set; }
    public Circle(double radius)
    {
        Radius = radius;
    }
    public override double Area()
    {
        return Math.PI * Radius * Radius;
    }
    public override double Perimeter()
    {
        return 2 * Math.PI * Radius;
    }
}
// Производный класс Rectangle (Прямоугольник)
public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }
    public override double Area()
    {
        return Width * Height;
    }
    public override double Perimeter()
    {
        return 2 * (Width + Height);
    }
}
class Program
{
    static void Main(string[] args)
    {
        // Создание объектов
        Shape shape = new Shape();
        Circle circle = new Circle(5);
        Rectangle rectangle = new Rectangle(4, 6);
        // Вывод информации о каждой фигуре
        Console.WriteLine("Базовый класс Shape");
        shape.PrintInfo();
        Console.WriteLine("Круг (радиус = 5)");
        circle.PrintInfo();
        Console.WriteLine("Прямоугольник (4 x 6)");
        rectangle.PrintInfo();
        // Демонстрация полиморфизма
        Console.WriteLine("Полиморфизм: массив Shape");
        Shape[] shapes = { shape, circle, rectangle };
        foreach (Shape s in shapes)
        {
            Console.WriteLine($"{s.GetType().Name}: Площадь = {s.Area():F2}, Периметр = {s.Perimeter():F2}");
        }
    }
}