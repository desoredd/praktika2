using StudentManager.Models;
namespace StudentManager.Repositories;
public interface IStudentRepository
{
    IReadOnlyList<Student> GetAll();
    Student? GetById(int id);
    void Add(Student student);
    bool Update(Student student);
    bool Delete(int id);
    int GetNextId();
    void Save();
}