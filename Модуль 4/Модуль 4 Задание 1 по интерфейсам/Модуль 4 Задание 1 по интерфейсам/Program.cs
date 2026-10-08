public interface Shape
{
    double Area();
    double Perimeter();
    string GetName();
    
}
public class Circle : Shape
{
    private int Rad;
    public int rad 
    {  get => Rad;
       set 
       {
            if(value <= 0)
            {
                Console.WriteLine("Введено некорректное значение радиуса оно будет заменено на 1");
                value = 1;
            }
            Rad = value;
        }
    }
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
    private int wid;
    private int hig;
    public int width
    { 
        get => wid;
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("Введено некорректное значение ширины оно будет заменено на 1");
                value = 1;
            }
            wid = value;
        }
    }
    public int hight
    { 
        get => hig; 
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("Введено некорректное значение высоты оно будет заменено на 1");
                value = 1;
            }
            hig = value;
        }
    }
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
    private int x;
    private int y;
    private int z;
    public int a
    {
        get => x;
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("Введено некорректное значение стороны оно будет заменено на 1");
                value = 1;
            }
            x = value;
        }
    }
    public int b
    {
        get => y;
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("Введено некорректное значение стороны оно будет заменено на 1");
                value = 1;
            }
            y = value;
        }
    }
    public int c
    {
        get => z;
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("Введено некорректное значение стороны оно будет заменено на 1");
                value = 1;
            }
            z = value;
        }
    }
    public Triangle(int A, int B, int C)
    {
        a = A;
        b = B;
        c = C;
        if (a + b <= c || a + c <= b || b + c <= a)
        {
            Console.WriteLine("Треугольник с такими сторонами не существует, их значение будет заменено на 1");
            int m = 1;
            a = b = c = m;
        }
    }
    public double Area()
    {
        double p = (a + b + c) / 2.0;
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
            new Triangle(1, 1, 10)
        };

        foreach (var shape in shapes)
        {
            Console.WriteLine($"{shape.GetName()}");
            Console.WriteLine($"    Площадь: {shape.Area():F2}");
            Console.WriteLine($"    Периметр: {shape.Perimeter():F2}");
        }
    }
}