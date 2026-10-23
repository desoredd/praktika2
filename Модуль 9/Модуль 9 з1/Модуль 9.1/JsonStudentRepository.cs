using System.Text.Json;
using StudentManager.Models;
namespace StudentManager.Repositories;
public class JsonStudentRepository : IStudentRepository
{
    private readonly string _filePath;
    private readonly List<Student> _students = new();
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    public JsonStudentRepository(string filePath = "students.json")
    {
        _filePath = filePath;
        Load();
    }
    private void Load()
    {
        if (!File.Exists(_filePath)) return;
        try
        {
            var json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<List<Student>>(json, JsonOptions);
            if (data != null) _students.AddRange(data);
        }
        catch (JsonException)
        {
            // повреждённый файл — начинаем с пустого списка
        }
    }
    public void Save()
    {
        var json = JsonSerializer.Serialize(_students, JsonOptions);
        File.WriteAllText(_filePath, json);
    }
    public IReadOnlyList<Student> GetAll() => _students.AsReadOnly();
    public Student? GetById(int id) => _students.FirstOrDefault(s => s.Id == id);
    public void Add(Student student) => _students.Add(student);
    public bool Update(Student student)
    {
        var index = _students.FindIndex(s => s.Id == student.Id);
        if (index < 0) return false;
        _students[index] = student;
        return true;
    }
    public bool Delete(int id)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        return student != null && _students.Remove(student);
    }
    public int GetNextId() => _students.Count == 0 ? 1 : _students.Max(s => s.Id) + 1;
}