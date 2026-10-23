using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
namespace Модуль_9
{
    public class TaskService
    {
        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();
        public bool Add(string text, TaskPriority priority, DateTime? dueDate)
        {
            text = (text ?? "").Trim();
            if (text.Length == 0) return false;
            Tasks.Add(new TaskItem(text, priority, dueDate));
            return true;
        }
        public bool Update(TaskItem item, string newText, TaskPriority priority, DateTime? dueDate)
        {
            if (item == null) return false;
            newText = (newText ?? "").Trim();
            if (newText.Length == 0) return false;
            item.Text = newText;
            item.Priority = priority;
            item.DueDate = dueDate;
            return true;
        }
        public bool DeleteSelected(TaskItem item)
        {
            if (item == null)
            {
                MessageBox.Show("Выберите задачу для удаления.",
                    "Ничего не выбрано", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            var r = MessageBox.Show(
                "Удалить задачу «" + item.Text + "»?",
                "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r != MessageBoxResult.Yes)
                return false;
            Tasks.Remove(item);
            return true;
        }
        public bool ToggleSelected(TaskItem item)
        {
            if (item == null) return false;
            item.Done = !item.Done;
            return true;
        }
        public bool ClearDone()
        {
            int removed = 0;
            for (int i = Tasks.Count - 1; i >= 0; i--)
            {
                if (Tasks[i].Done)
                {
                    Tasks.RemoveAt(i);
                    removed++;
                }
            }
            if (removed == 0)
            {
                MessageBox.Show("Выполненных задач нет.",
                    "Нечего удалять", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }
        public string GetStatusText()
        {
            int total = Tasks.Count;
            int done = Tasks.Count(t => t.Done);
            int overdue = Tasks.Count(t => t.IsOverdue);
            int dueSoon = Tasks.Count(t => t.IsDueSoon);
            string result = string.Format(
                "Всего: {0}   |   Выполнено: {1}   |   Осталось: {2}",
                total, done, total - done);
            if (overdue > 0) result += "   |   ⚠ Просрочено: " + overdue;
            if (dueSoon > 0) result += "   |   ⏰ Скоро: " + dueSoon;
            return result;
        }
        /// Доля выполненных задач (0..1) для прогресс-бара.
        public double GetProgress()
        {
            if (Tasks.Count == 0) return 0;
            return (double)Tasks.Count(t => t.Done) / Tasks.Count;
        }
        /// Задачи, срок которых наступает в ближайшие N дней (или уже просрочен).
        public System.Collections.Generic.List<TaskItem> GetUpcoming(int days)
        {
            var today = DateTime.Today;
            return Tasks
                .Where(t => !t.Done && t.DueDate.HasValue &&
                            (t.DueDate.Value.Date - today).TotalDays <= days)
                .OrderBy(t => t.DueDate)
                .ToList();
        }
    }
}