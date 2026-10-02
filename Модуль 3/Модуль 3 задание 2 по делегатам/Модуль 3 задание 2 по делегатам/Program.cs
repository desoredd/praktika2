// Аргументы события — данные уведомления
public class NotificationEventArgs : EventArgs
{
    public string Message { get; }
    public DateTime Timestamp { get; }
    public NotificationPriority Priority { get; }
    public NotificationEventArgs(string message, NotificationPriority priority = NotificationPriority.Normal)
    {
        Message = message;
        Timestamp = DateTime.Now;
        Priority = priority;
    }
}
// Приоритет уведомления
public enum NotificationPriority
{
    Low,
    Normal,
    High,
    Urgent
}
// Класс "Уведомление" — источник событий
public class Notification
{
    // События для разных типов уведомлений
    public event EventHandler<NotificationEventArgs> MessageReceived;
    public event EventHandler<NotificationEventArgs> CallReceived;
    public event EventHandler<NotificationEventArgs> EmailReceived;
    // Методы-триггеры для запуска событий
    public void SendMessage(string text, NotificationPriority priority = NotificationPriority.Normal)
    {
        Console.WriteLine($"\nОтправка СООБЩЕНИЯ: \"{text}\"");
        OnMessageReceived(new NotificationEventArgs(text, priority));
    }
    public void MakeCall(string contact, NotificationPriority priority = NotificationPriority.High)
    {
        Console.WriteLine($"\nВходящий ЗВОНОК от: {contact}");
        OnCallReceived(new NotificationEventArgs($"Звонок от {contact}", priority));
    }
    public void SendEmail(string subject, string body, NotificationPriority priority = NotificationPriority.Normal)
    {
        Console.WriteLine($"\nПолучено EMAIL: \"{subject}\"");
        OnEmailReceived(new NotificationEventArgs($"Email: {subject}\n     {body}", priority));
    }
    // Защищённые методы для вызова событий (паттерн OnXxx)
    protected virtual void OnMessageReceived(NotificationEventArgs e)
    {
        MessageReceived?.Invoke(this, e);
    }
    protected virtual void OnCallReceived(NotificationEventArgs e)
    {
        CallReceived?.Invoke(this, e);
    }
    protected virtual void OnEmailReceived(NotificationEventArgs e)
    {
        EmailReceived?.Invoke(this, e);
    }
}
// Подписчики — обработчики событий
public class MessageHandler
{
    public void OnMessageReceived(object sender, NotificationEventArgs e)
    {
        Console.WriteLine($"[SMS-модуль] {e.Timestamp:HH:mm:ss} | Приоритет: {e.Priority}");
        Console.WriteLine($"Текст: {e.Message}");
    }
}
public class CallHandler
{
    public void OnCallReceived(object sender, NotificationEventArgs e)
    {
        Console.WriteLine($"[Телефон] {e.Timestamp:HH:mm:ss} | Приоритет: {e.Priority}");
        Console.WriteLine($"{e.Message}");
        Console.WriteLine("Воспроизведение рингтона...");
    }
}
public class EmailHandler
{
    public void OnEmailReceived(object sender, NotificationEventArgs e)
    {
        Console.WriteLine($"[Почта] {e.Timestamp:HH:mm:ss} | Приоритет: {e.Priority}");
        Console.WriteLine($"{e.Message}");
    }
}
// Дополнительный подписчик — журнал (логирование всех событий)
public class NotificationLogger
{
    public void Log(object sender, NotificationEventArgs e)
    {
        Console.WriteLine($"[LOG] Тип события от {sender.GetType().Name}, приоритет {e.Priority}");
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        // Создаём источник событий
        Notification notification = new Notification();
        // Создаём обработчиков
        var messageHandler = new MessageHandler();
        var callHandler = new CallHandler();
        var emailHandler = new EmailHandler();
        var logger = new NotificationLogger();
        // === Регистрация обработчиков событий ===
        notification.MessageReceived += messageHandler.OnMessageReceived;
        notification.CallReceived += callHandler.OnCallReceived;
        notification.EmailReceived += emailHandler.OnEmailReceived;
        // Дополнительные подписчики (логирование)
        notification.MessageReceived += logger.Log;
        notification.CallReceived += logger.Log;
        notification.EmailReceived += logger.Log;
        // === Запуск событий ===
        Console.WriteLine("СИСТЕМА УВЕДОМЛЕНИЙ");
        notification.SendMessage("Привет! Как дела?", NotificationPriority.Normal);
        notification.MakeCall("+375 (29) 123-45-67");
        notification.SendEmail("Совещание", "Встреча в 15:00 в переговорной №3");
        notification.SendMessage("СРОЧНО: отчёт готов?", NotificationPriority.Urgent);
        // === Отписка одного из обработчиков ===
        Console.WriteLine("\nОтписываем SMS-модуль");
        notification.MessageReceived -= messageHandler.OnMessageReceived;
        notification.SendMessage("Это сообщение уже не увидит SMS-модуль",
                                 NotificationPriority.Low);
        Console.ReadKey();
    }
}