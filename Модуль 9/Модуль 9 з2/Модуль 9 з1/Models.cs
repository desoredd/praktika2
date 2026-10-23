namespace ProjectManagerWpf
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Position { get; set; } = "";
        public string Email { get; set; } = "";
        public override string ToString()
        {
            return FullName;
        }
    }
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public System.DateTime StartDate { get; set; } = System.DateTime.Today;
        public System.DateTime? EndDate { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    public enum TaskStatus
    {
        New,
        InProgress,
        Done,
        Cancelled
    }
    public class TaskItem
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int? EmployeeId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public TaskStatus Status { get; set; } = TaskStatus.New;
        public int Progress { get; set; }
        public System.DateTime CreatedAt { get; set; } = System.DateTime.Now;
        // Навигационные поля для отображения в таблице
        public string ProjectName { get; set; } = "";
        public string EmployeeName { get; set; } = "";
    }
}