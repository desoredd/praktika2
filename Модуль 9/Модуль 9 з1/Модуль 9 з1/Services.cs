using System;
using System.Linq;
using System.Text;
namespace ProjectManagerWpf
{
    public class Services
    {
        public EmployeeRepository Employees { get; private set; }
        public ProjectRepository Projects { get; private set; }
        public TaskRepository Tasks { get; private set; }
        public Services(Database db)
        {
            Employees = new EmployeeRepository(db);
            Projects = new ProjectRepository(db);
            Tasks = new TaskRepository(db);
        }
        public void AssignTask(int taskId, int? employeeId)
        {
            var task = Tasks.GetAll().FirstOrDefault(t => t.Id == taskId);
            if (task == null) throw new InvalidOperationException("Задача не найдена.");
            if (employeeId.HasValue &&
                Employees.GetAll().All(e => e.Id != employeeId.Value))
                throw new InvalidOperationException("Сотрудник не найден.");
            task.EmployeeId = employeeId;
            Tasks.Update(task);
        }
        public void UpdateProgress(int taskId, int progress)
        {
            var task = Tasks.GetAll().FirstOrDefault(t => t.Id == taskId);
            if (task == null) throw new InvalidOperationException("Задача не найдена.");
            task.Progress = Math.Max(0, Math.Min(100, progress));
            if (task.Progress == 0) task.Status = TaskStatus.New;
            else if (task.Progress == 100) task.Status = TaskStatus.Done;
            else task.Status = TaskStatus.InProgress;
            Tasks.Update(task);
        }
        public string BuildProjectReport(int projectId)
        {
            var project = Projects.GetAll().FirstOrDefault(p => p.Id == projectId);
            if (project == null) return "Проект не найден.";
            var tasks = Tasks.GetAll().Where(t => t.ProjectId == projectId).ToList();
            var sb = new StringBuilder();
            sb.AppendLine(new string('=', 70));
            sb.AppendLine("ОТЧЁТ ПО ПРОЕКТУ #" + project.Id + ": " + project.Name);
            sb.AppendLine(new string('=', 70));
            sb.AppendLine("Описание:  " + project.Description);
            sb.AppendLine("Старт:     " + project.StartDate.ToString("yyyy-MM-dd"));
            sb.AppendLine("Окончание: " +
                (project.EndDate.HasValue ? project.EndDate.Value.ToString("yyyy-MM-dd") : "—"));
            sb.AppendLine(new string('-', 70));
            if (tasks.Count == 0)
            {
                sb.AppendLine("Задач по проекту нет.");
                return sb.ToString();
            }
            foreach (var t in tasks)
                sb.AppendLine(string.Format("{0,-4} [{1,-10}] {2,3}%  {3}",
                    t.Id, t.Status.ToString(), t.Progress, t.Title));
            sb.AppendLine(new string('-', 70));
            sb.AppendLine("Всего задач:        " + tasks.Count);
            sb.AppendLine("Выполнено:          " + tasks.Count(t => t.Status == TaskStatus.Done));
            sb.AppendLine("В работе:           " + tasks.Count(t => t.Status == TaskStatus.InProgress));
            sb.AppendLine("Средний прогресс:   " +
                tasks.Average(t => t.Progress).ToString("F1") + "%");
            return sb.ToString();
        }
        public string BuildAllProjectsReport()
        {
            var projects = Projects.GetAll();
            var allTasks = Tasks.GetAll();
            var sb = new StringBuilder();
            sb.AppendLine(new string('=', 70));
            sb.AppendLine("СВОДНЫЙ ОТЧЁТ ПО ПРОЕКТАМ");
            sb.AppendLine(new string('=', 70));
            foreach (var p in projects)
            {
                var tasks = allTasks.Where(t => t.ProjectId == p.Id).ToList();
                var done = tasks.Count(t => t.Status == TaskStatus.Done);
                var avg = tasks.Count > 0 ? tasks.Average(t => t.Progress) : 0;
                sb.AppendLine(string.Format("#{0,-3} {1,-25} задач: {2,-3} выполнено: {3,-3} ср. прогресс: {4,5:F1}%",
                    p.Id, p.Name, tasks.Count, done, avg));
            }
            return sb.ToString();
        }
        public string BuildEmployeeReport(int employeeId)
        {
            var emp = Employees.GetAll().FirstOrDefault(e => e.Id == employeeId);
            if (emp == null) return "Сотрудник не найден.";
            var tasks = Tasks.GetAll().Where(t => t.EmployeeId == employeeId).ToList();
            var sb = new StringBuilder();
            sb.AppendLine(new string('=', 70));
            sb.AppendLine("ОТЧЁТ ПО СОТРУДНИКУ #" + emp.Id + ": " + emp.FullName + " (" + emp.Position + ")");
            sb.AppendLine(new string('=', 70));
            if (tasks.Count == 0)
            {
                sb.AppendLine("Задач не назначено.");
                return sb.ToString();
            }
            foreach (var t in tasks)
                sb.AppendLine(string.Format("{0,-4} [{1,-10}] {2,3}%  {3}  ({4})",
                    t.Id, t.Status.ToString(), t.Progress, t.Title, t.ProjectName));
            sb.AppendLine(new string('-', 70));
            sb.AppendLine("Всего задач: " + tasks.Count);
            sb.AppendLine("Активных:    " + tasks.Count(t =>
                t.Status == TaskStatus.New || t.Status == TaskStatus.InProgress));
            sb.AppendLine("Выполнено:   " + tasks.Count(t => t.Status == TaskStatus.Done));
            return sb.ToString();
        }
    }
}