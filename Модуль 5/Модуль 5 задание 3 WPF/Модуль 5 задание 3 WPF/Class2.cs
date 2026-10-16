using System.Collections.ObjectModel;
using System.Windows;
namespace Модуль_5_задание_3
{
    public class TaskService
    {
        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();
        // Добавление
        // Возвращает true, если задача действительно добавлена
        public bool Add(string text)
        {
            text = (text ?? "").Trim();
            if (text.Length == 0) return false;

            Tasks.Add(new TaskItem(text));
            return true;
        }
        // Удаление выбранной
        // Возвращает true, если задача удалена
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
        // Переключить выполнение
        public bool ToggleSelected(TaskItem item)
        {
            if (item == null) return false;

            item.Done = !item.Done;
            return true;
        }
        // Очистить выполненные
        // Возвращает true, если что-то удалили
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
        // Статистика для строки статуса
        public string GetStatusText()
        {
            int total = Tasks.Count;
            int done = 0;
            foreach (var t in Tasks)
                if (t.Done) done++;
            return string.Format(
                "Всего: {0}   |   Выполнено: {1}   |   Осталось: {2}",
                total, done, total - done);
        }
    }
}