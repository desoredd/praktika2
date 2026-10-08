using System;
using System.Collections.Generic;
public interface Tovar
{
    double Price();
    string Category();
    int Numbers();
}
public class Voda : Tovar
{
    private double prc;
    private double dsc;
    private int cnt;
    public double price
    {
        get => prc;
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("Введена некорректная цена, она будет заменена на 1");
                value = 1;
            }
            prc = value;
        }
    }
    public double amount
    {
        get => dsc;
        set
        {
            if (value < 0)
            {
                Console.WriteLine("Введена некорректная скидка, она будет заменена на 0");
                value = 0;
            }
            if (value > 100)
            {
                Console.WriteLine("Скидка не может быть больше 100%, она будет заменена на 100");
                value = 100;
            }
            dsc = value;
        }
    }
    public int num
    {
        get => cnt;
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("Введено некорректное количество, оно будет заменено на 1");
                value = 1;
            }
            cnt = value;
        }
    }
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
    public string Category() => "Вода";
    public int Numbers() => num;
}
public class Sok : Tovar
{
    private double prc;
    private double dsc;
    private int cnt;
    public double price
    {
        get => prc;
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("Введена некорректная цена, она будет заменена на 1");
                value = 1;
            }
            prc = value;
        }
    }
    public double amount
    {
        get => dsc;
        set
        {
            if (value < 0)
            {
                Console.WriteLine("Введена некорректная скидка, она будет заменена на 0");
                value = 0;
            }
            if (value > 100)
            {
                Console.WriteLine("Скидка не может быть больше 100%, она будет заменена на 100");
                value = 100;
            }
            dsc = value;
        }
    }
    public int num
    {
        get => cnt;
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("Введено некорректное количество, оно будет заменено на 1");
                value = 1;
            }
            cnt = value;
        }
    }
    public Sok(int num, double price, double amount)
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
    public string Category() => "Сок";
    public int Numbers() => num;
}
class Program
{
    static void Main()
    {
        var tovars = new List<Tovar>
        {
            new Voda(10, 12.50, 15),
            new Sok(2, 15, 20)
        };
        bool boo = true;
        while (boo)
        {
            Console.WriteLine("\nМеню");
            Console.WriteLine("1 - Показать цены");
            Console.WriteLine("2 - Показать количество");
            Console.WriteLine("3 - Добавить товар (Вода)");
            Console.WriteLine("4 - Добавить товар (Сок)");
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
                    AddTovar(tovars, true);
                    break;
                case "4":
                    AddTovar(tovars, false);
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
    static void AddTovar(List<Tovar> tovars, bool isVoda)
    {
        Console.Write("Введите количество: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Ошибка: количество должно быть целым числом.");
            return;
        }
        Console.Write("Введите цену: ");
        if (!double.TryParse(Console.ReadLine(), out double price))
        {
            Console.WriteLine("Ошибка: цена должна быть числом.");
            return;
        }
        Console.Write("Введите скидку (в %): ");
        if (!double.TryParse(Console.ReadLine(), out double amount))
        {
            Console.WriteLine("Ошибка: скидка должна быть числом.");
            return;
        }
        if (isVoda)
        {
            tovars.Add(new Voda(id, price, amount));
            Console.WriteLine("Вода успешно добавлена!");
        }
        else
        {
            tovars.Add(new Sok(id, price, amount));
            Console.WriteLine("Сок успешно добавлен!");
        }
    }
}