// Класс-аргумент события: содержит данные о температуре
public class TemperatureChangedEventArgs : EventArgs
{
    public double OldTemperature { get; }
    public double NewTemperature { get; }
    public TemperatureChangedEventArgs(double oldTemp, double newTemp)
    {
        OldTemperature = oldTemp;
        NewTemperature = newTemp;
    }
}
// Издатель: датчик температуры
public class TemperatureSensor
{
    private double temp;
    // Событие на основе стандартного делегата EventHandler<T>
    public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged;
    public double Temperature
    {
        get => temp;
        set
        {
            if (Math.Abs(temp - value) > 0.001)
            {
                double oldTemp = temp;
                temp = value;

                // Уведомляем подписчиков
                OnTemperatureChanged(new TemperatureChangedEventArgs(oldTemp, temp));
            }
        }
    }
    // Защищённый метод для генерации события (стандартный паттерн)
    protected virtual void OnTemperatureChanged(TemperatureChangedEventArgs e)
    {
        TemperatureChanged.Invoke(this, e);
    }
}
// Подписчик: термостат
public class Thermostat
{
    public string Name { get; }
    public double MinTemperature { get; set; }
    public double MaxTemperature { get; set; }
    public bool HeatingOn { get; private set; }
    public Thermostat(string name, double min, double max)
    {
        Name = name;
        MinTemperature = min;
        MaxTemperature = max;
        HeatingOn = false;
    }
    // Обработчик события
    public void OnTemperatureChanged(object sender, TemperatureChangedEventArgs e)
    {
        Console.WriteLine($"[{Name}] Температура: {e.OldTemperature:F1}°C  {e.NewTemperature:F1}°C");
        if (e.NewTemperature < MinTemperature && !HeatingOn)
        {
            HeatingOn = true;
            Console.WriteLine($"[{Name}] Слишком холодно! Отопление ВКЛЮЧЕНО.");
        }
        else if (e.NewTemperature > MaxTemperature && HeatingOn)
        {
            HeatingOn = false;
            Console.WriteLine($"[{Name}] Слишком жарко! Отопление ВЫКЛЮЧЕНО.");
        }
        else
        {
            Console.WriteLine($"[{Name}] Отопление {(HeatingOn ? "включено" : "выключено")}. " +
                              $"Температура в норме.");
        }
        Console.WriteLine();
    }
}
class Program
{
    static void Main()
    {
        // Создаём датчик и термостаты
        TemperatureSensor sensor = new TemperatureSensor();
        Thermostat room1 = new Thermostat("Гостиная", 20.0, 25.0);
        Thermostat room2 = new Thermostat("Спальня", 18.0, 24.0);
        // Подписываемся на событие
        sensor.TemperatureChanged += room1.OnTemperatureChanged;
        sensor.TemperatureChanged += room2.OnTemperatureChanged;
        // Изменяем температуру — событие будет сгенерировано автоматически
        Console.WriteLine("Начальная температура");
        sensor.Temperature = 22.0;
        Console.WriteLine("Температура падает");
        sensor.Temperature = 19.0;
        Console.WriteLine("Температура падает ещё сильнее");
        sensor.Temperature = 16.0;
        Console.WriteLine("Температура растёт");
        sensor.Temperature = 21.0;
        Console.WriteLine("Температура становится жаркой");
        sensor.Temperature = 26.0;
        // Отписываемся (опционально)
        Console.WriteLine("Отписываем спальню от события");
        sensor.TemperatureChanged -= room2.OnTemperatureChanged;
        sensor.Temperature = 15.0;
    }
}