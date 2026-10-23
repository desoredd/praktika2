namespace StudentManager.Models;
public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Group { get; set; } = string.Empty;
    public double AverageGrade { get; set; }

    public Student Clone() => (Student)MemberwiseClone();

    public override string ToString() =>
        $"ID: {Id,-4} | {LastName} {FirstName,-15} | " +
        $"Возраст: {Age,-3} | Группа: {Group,-10} | " +
        $"Средний балл: {AverageGrade:F2}";
}