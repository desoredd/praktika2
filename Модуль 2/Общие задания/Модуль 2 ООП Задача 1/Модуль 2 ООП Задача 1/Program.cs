class Program
{
    public partial class Person()
    {
        string name {  get; set; }
        int age { get; set; }
        string adres {  get; set; }
        public static void Pasport(Person a)
        {
            Console.Write("Введите свое имя: ");
            a.name = Console.ReadLine();
            Console.Write("Введите свой возраст: ");
            a.age = System.Convert.ToInt16(Console.ReadLine());
            Console.Write("Введите свой адрес: ");
            a.adres = Console.ReadLine();
        }
        public static void Vicheslen(Person a)
        {
            Console.WriteLine($"Здравствуйте {a.name}, вы еще совсем молоды ведь вам всего {a.age} лет, я знаю ваш адрес {a.adres}, скоро заеду в гости.");
        }

    }
    static void Main()
    {
        Person a = new Person();
        Person.Pasport(a);
        Person.Vicheslen(a);
    }
}