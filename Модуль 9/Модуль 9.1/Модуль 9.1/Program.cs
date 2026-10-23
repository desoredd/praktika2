using StudentManager.Repositories;
using StudentManager.Services;
using StudentManager.UI;
namespace StudentManager;
public static class Program
{
    public static void Main()
    {
        IStudentRepository repository = new JsonStudentRepository("students.json");
        var service = new StudentService(repository);
        var ui = new ConsoleUI(service);
        ui.Run();
    }
}