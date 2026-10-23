using StudentManager.Models;
using StudentManager.Repositories;
namespace StudentManager.Services;
public class StudentService
{
    private readonly IStudentRepository _repository;
    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }
    // CRUD
    public Student AddStudent(string firstName, string lastName,
                              int age, string group, double averageGrade)
    {
        var student = new Student
        {
            Id = _repository.GetNextId(),
            FirstName = firstName,
            LastName = lastName,
            Age = age,
            Group = group,
            AverageGrade = averageGrade
        };
        _repository.Add(student);
        _repository.Save();
        return student;
    }
    public bool UpdateStudent(int id, Action<Student> updateAction)
    {
        var student = _repository.GetById(id);
        if (student == null) return false;
        // Работаем с копией, чтобы избежать частичного изменения
        var updated = student.Clone();
        updateAction(updated);
        updated.Id = id;
        var result = _repository.Update(updated);
        if (result) _repository.Save();
        return result;
    }
    public bool DeleteStudent(int id)
    {
        var result = _repository.Delete(id);
        if (result) _repository.Save();
        return result;
    }
    public IReadOnlyList<Student> GetAllStudents() => _repository.GetAll();
    public Student? GetStudent(int id) => _repository.GetById(id);
    // Поиск
    public IReadOnlyList<Student> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAllStudents();
        var q = query.Trim().ToLowerInvariant();
        return _repository.GetAll()
            .Where(s =>
                s.FirstName.ToLowerInvariant().Contains(q) ||
                s.LastName.ToLowerInvariant().Contains(q) ||
                s.Group.ToLowerInvariant().Contains(q))
            .ToList();
    }
    // Сортировка
    public static readonly IReadOnlyDictionary<string, string> SortCriteria =
        new Dictionary<string, string>
        {
            ["1"] = "Id",
            ["2"] = "Фамилия",
            ["3"] = "Имя",
            ["4"] = "Возраст",
            ["5"] = "Группа",
            ["6"] = "Средний балл"
        };
    public IReadOnlyList<Student> SortStudents(string key, bool descending = false)
    {
        Func<Student, object> selector = key switch
        {
            "1" => s => s.Id,
            "2" => s => s.LastName,
            "3" => s => s.FirstName,
            "4" => s => s.Age,
            "5" => s => s.Group,
            "6" => s => s.AverageGrade,
            _ => throw new ArgumentException($"Неизвестный критерий сортировки: {key}")
        };
        var sorted = descending
            ? _repository.GetAll().OrderByDescending(selector)
            : _repository.GetAll().OrderBy(selector);
        return sorted.ToList();
    }
}