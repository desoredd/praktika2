public interface Tovar
{
    double Price();
    string Category();
    string Numbers();
}
public class Voda : Tovar
{
    public double price { get; set; }
    public double amount { get; set; }
    public int num { get; set; }
    public Voda(int num, double price, double amount)
    {
        this.num = num;
        this.price = price;
        this.amount = amount;
    }
    public double Price()
    {
        double AllPrice = price - (price / 100) * amount;
        return AllPrice;
    }
    public string Category() => "Напитки";
    public string Numbers() => num.ToString();
}
public class Vodka : Tovar
{
    public double price { get; set; }
    public double amount { get; set; }
    public int num { get; set; }
    public Vodka(int num, double price, double amount)
    {
        this.num = num;
        this.price = price;
        this.amount = amount;
    }
    public double Price()
    {
        double AllPrice = price - (price / 100) * amount;
        return AllPrice;
    }
    public string Category() => "Напитки";
    public string Numbers() => num.ToString();
}
class Program
{
    static void Main()
    {
        var tovars = new List<Tovar>
        {
            new Voda(10, 12.50, 15),
            new Vodka(2, 15, 20)
        };
        bool boo = true;
        while (boo)
        {
            Console.WriteLine("Меню");
            Console.WriteLine("1 - Показать цены");
            Console.WriteLine("2 - Показать количество");
            Console.WriteLine("3 - Добавить товар (Вода)");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    foreach (var tovar in tovars)
                    {
                        Console.WriteLine($"Товар: {tovar.Category()}. Цена = {tovar.Price():F2}");
                    }
                    break;
                case "2":
                    foreach (var tovar in tovars)
                    {
                        Console.WriteLine($"Товар: {tovar.Category()}. Кол-во на складе: {tovar.Numbers()}");
                    }
                    break;
                case "3":
                    Console.Write("Введите колличество: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("Ошибка: колличество должено быть целым числом.");
                        break;
                    }
                    Console.Write("Введите цену: ");
                    if (!double.TryParse(Console.ReadLine(), out double price))
                    {
                        Console.WriteLine("Ошибка: цена должна быть числом.");
                        break;
                    }
                    Console.Write("Введите скидку (в %): ");
                    if (!double.TryParse(Console.ReadLine(), out double amount))
                    {
                        Console.WriteLine("Ошибка: скидка должна быть числом.");
                        break;
                    }
                    tovars.Add(new Voda(id, price, amount));
                    Console.WriteLine("Товар успешно добавлен!");
                    break;
                case "0":
                    boo = false;
                    Console.WriteLine("Выход из программы...");
                    break;
                default:
                    Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                    break;
            }
        }
    }
}