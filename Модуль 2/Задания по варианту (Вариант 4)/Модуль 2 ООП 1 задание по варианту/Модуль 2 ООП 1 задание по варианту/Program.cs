public class BankAccount
{
    private static int nextId = 1000;
    private readonly int number;
    private string owner;
    private decimal money;
    public int Number => number;
    public decimal Money => money;
    public string Owner
    {
        get => owner;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Имя владельца не может быть пустым.");
            owner = value.Trim();
        }
    }
    public BankAccount(string owner)
        : this(owner, 0m)
    {
    }
    public BankAccount(string owner, decimal money)
    {
        number = nextId++;
        Owner = owner;
        if (money < 0)
            throw new ArgumentException("Начальный баланс не может быть отрицательным.");
        this.money = money;
    }
    public void Put(decimal sum)
    {
        if (sum <= 0)
            throw new ArgumentException("Сумма пополнения должна быть положительной.");
        money += sum;
        Console.WriteLine($"[Счёт #{number}] Пополнение на {sum:C}. Баланс: {money:C}");
    }
    public bool Take(decimal sum)
    {
        if (sum <= 0)
            throw new ArgumentException("Сумма снятия должна быть положительной.");
        if (sum > money)
        {
            Console.WriteLine($"[Счёт #{number}] Недостаточно средств. " +
                              $"Запрошено: {sum:C}, доступно: {money:C}");
            return false;
        }
        money -= sum;
        Console.WriteLine($"[Счёт #{number}] Снятие {sum:C}. Баланс: {money:C}");
        return true;
    }
    public bool Send(BankAccount to, decimal sum)
    {
        if (to == null)
            throw new ArgumentNullException(nameof(to));
        if (to == this)
            throw new InvalidOperationException("Нельзя перевести средства на тот же счёт.");
        Console.WriteLine($"\n[Перевод] {owner} → {to.owner}: {sum:C}");
        if (!Take(sum))
            return false;
        to.Put(sum);
        return true;
    }
    public void Show()
    {
        Console.WriteLine($"Счёт #{number} Владелец: {owner} Баланс: {money:C}");
    }
    public override string ToString()
    {
        return $"BankAccount(#{number}, {owner}, {money:C})";
    }
}
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Создание счетов");
        var a1 = new BankAccount("Иван Иванов", 1000m);
        var a2 = new BankAccount("Мария Петрова", 500m);
        var a3 = new BankAccount("Пётр Сидоров");
        a1.Show();
        a2.Show();
        a3.Show();
        Console.WriteLine();
        Console.WriteLine("Пополнение");
        a1.Put(250m);
        a3.Put(10000m);
        Console.WriteLine();
        Console.WriteLine("Снятие");
        a1.Take(300m);
        a2.Take(1000m);
        Console.WriteLine();
        Console.WriteLine("Перевод между счетами");
        a3.Send(a2, 5000m);
        Console.WriteLine();
        Console.WriteLine("Итоговое состояние");
        a1.Show();
        a2.Show();
        a3.Show();
        Console.WriteLine();
        Console.WriteLine("Проверка уникальности номеров");
        var a4 = new BankAccount("Тест");
        Console.WriteLine($"a1: #{a1.Number}, a2: #{a2.Number}, a3: #{a3.Number}, a4: #{a4.Number}");
        Console.WriteLine("\nОбработка исключений");
        try
        {
            a1.Put(-100m);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        try
        {
            var bad = new BankAccount("");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}