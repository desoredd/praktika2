using System.Collections;
using System.Text;
public class StringArray : IEnumerable<string>
{
    private readonly string[] data;
    private readonly int width;
    public StringArray(int size, int width)
    {
        if (size < 0)
            throw new ArgumentException("Размер массива не может быть отрицательным.", nameof(size));
        if (width <= 0)
            throw new ArgumentException("Длина строки должна быть положительной.", nameof(width));
        this.data = new string[size];
        this.width = width;
        for (int i = 0; i < size; i++)
            data[i] = new string(' ', width);
    }
    public StringArray(string[] source, int width)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (width <= 0)
            throw new ArgumentException("Длина строки должна быть положительной.", nameof(width));
        this.width = width;
        data = new string[source.Length];
        for (int i = 0; i < source.Length; i++)
            data[i] = Fit(source[i] ?? string.Empty, width);
    }
    public int Length => data.Length;
    public int Width  => width;
    public string this[int index]
    {
        get
        {
            CheckIndex(index);
            return data[index];
        }
        set
        {
            CheckIndex(index);
            data[index] = Fit(value ?? string.Empty, width);
        }
    }
    private void CheckIndex(int index)
    {
        if (index < 0 || index >= data.Length)
            throw new IndexOutOfRangeException(
                $"Индекс {index} выходит за границы массива (допустимо: 0..{data.Length - 1}).");
    }
    private static string Fit(string s, int length)
    {
        if (s.Length == length) return s;
        if (s.Length > length)  return s.Substring(0, length);
        return s.PadRight(length);
    }
    public static StringArray ConcatElementwise(StringArray a, StringArray b)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        if (b == null) throw new ArgumentNullException(nameof(b));
        int size  = Math.Max(a.Length, b.Length);
        int w     = Math.Max(a.width, b.width);
        var res   = new StringArray(size, w);
        for (int i = 0; i < size; i++)
        {
            string left  = i < a.Length ? a[i].TrimEnd() : string.Empty;
            string right = i < b.Length ? b[i].TrimEnd() : string.Empty;
            res[i] = left + right;
        }
        return res;
    }
    public static StringArray operator +(StringArray a, StringArray b)
        => ConcatElementwise(a, b);
    public static StringArray MergeDistinct(StringArray a, StringArray b)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        if (b == null) throw new ArgumentNullException(nameof(b));
        int w = Math.Max(a.width, b.width);
        var list = new List<string>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        void Add(string s)
        {
            string fitted = Fit(s, w);
            if (seen.Add(fitted))
                list.Add(fitted);
        }
        foreach (var s in a) Add(s.TrimEnd());
        foreach (var s in b) Add(s.TrimEnd());
        return new StringArray(list.ToArray(), w);
    }
    public void PrintItem(int index)
    {
        CheckIndex(index);
        Console.WriteLine($"[{index}] = \"{data[index]}\"");
    }
    public void PrintAll()
    {
        Console.WriteLine($"Массив строк (длина = {Length}, размер строки = {Width})");
        if (data.Length == 0)
        {
            Console.WriteLine("  (пусто)\n");
            return;
        }
        for (int i = 0; i < data.Length; i++)
            Console.WriteLine($"  [{i,2}] \"{data[i]}\"");
        Console.WriteLine();
    }
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append('[');
        for (int i = 0; i < data.Length; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append('"').Append(data[i]).Append('"');
        }
        sb.Append(']');
        return sb.ToString();
    }
    public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)data).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()     => data.GetEnumerator();
}
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Создание массива строк (размер 5, длина строки 20)");
        var a = new StringArray(5, 20);
        a[0] = "Привет ";
        a[1] = "Мир ";
        a[2] = "C# ";
        a[3] = "Очень Длинная Строка";
        a[4] = "Массив ";
        a.PrintAll();
        Console.WriteLine("Обращение по индексу");
        a.PrintItem(0);
        a.PrintItem(3);
        Console.WriteLine();
        Console.WriteLine("Контроль выхода за границы");
        try { a.PrintItem(100); }
        catch (IndexOutOfRangeException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        try { a[-1] = "test"; }
        catch (IndexOutOfRangeException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        Console.WriteLine();
        Console.WriteLine("Второй массив");
        var b = new StringArray(new[] { "Красный", "Синий", "C#", "Зелёный" }, 10);
        b.PrintAll();
        Console.WriteLine("Поэлементное сцепление a + b");
        var concat = a + b;
        concat.PrintAll();
        Console.WriteLine("Слияние a и b без дубликатов");
        var merged = StringArray.MergeDistinct(a, b);
        merged.PrintAll();
        Console.WriteLine("Перебор через foreach");
        foreach (var s in a)
            Console.WriteLine($"  \"{s}\"");
        Console.WriteLine();
        Console.WriteLine("LINQ: строки, содержащие «C#»");
        foreach (var s in a.Where(s => s.Contains("C#")))
            Console.WriteLine($"  \"{s}\"");
    }
}