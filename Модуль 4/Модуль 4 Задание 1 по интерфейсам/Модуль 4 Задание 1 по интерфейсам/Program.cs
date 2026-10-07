public interface Shape
{
    double Area();
    double Perimeter();
    string GetName();
    
}

public class Circle : Shape
{

    public int rad {  get; set; }
    public Circle(int Rad)
    { 
        rad = Rad;
    }
    public double Area()
    {
        double s = Math.PI * Math.Pow(rad,2);
        return s;
    }
    public double Perimeter()
    {
        double p = 2 * Math.PI * rad;
        return p;
    }
    public string GetName() => "Круг";
}

public class Rectangle : Shape
{
    public int width { get; set; }
    public int hight { get; set; }
    public Rectangle(int Width, int Hight)
    {
        width = Width;
        hight = Hight;
    }
    public double Area()
    {
        int s = width * hight;
        return s;
    }
    public double Perimeter()
    {
        int p = (width + hight) * 2;
        return p;
    }
    public string GetName() => "Прямоугольник";
}
public class Triangle : Shape
{
    public int a { get; set; }
    public int b { get; set; }
    public int c { get; set; }
    public Triangle(int A, int B, int C)
    {
        a = A;
        b = B;
        c = C;
    }
    public double Area()
    {
        double p = (a + b + c) / 2;
        double s = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        return s;
    }
    public double Perimeter()
    {
        int p = a + b + c;
        return p;
    }
    public string GetName() => "Треугольник";
}
class Program
{
    static void Main()
    {
        Console.WriteLine("Инфориация о фигурах");
        var shapes = new List<Shape>
        {
            new Circle(8),
            new Rectangle(5, 6),
            new Triangle(5, 8, 9)
        };

        foreach (var shape in shapes)
        {
            Console.WriteLine($"{shape.GetName()}");
            Console.WriteLine($"    Площадь: {shape.Area():F2}");
            Console.WriteLine($"    Периметр: {shape.Perimeter():F2}");
        }
    }
}