using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
namespace Модуль_5_задание_3
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TaskItem> tasks = new ObservableCollection<TaskItem>();
        public MainWindow()
        {
            InitializeComponent();
            lstTasks.ItemsSource = tasks;
            RefreshStatus();
        }
        // Добавить
        private void BtnAdd_Click(object sender, RoutedEventArgs e) => AddTask();
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddTask();
                e.Handled = true;
            }
        }
        private void AddTask()
        {
            string text = txtInput.Text.Trim();
            if (text.Length == 0) return;

            tasks.Add(new TaskItem(text));
            txtInput.Clear();
            txtInput.Focus();
            RefreshStatus();
        }
        // Удалить
        private void BtnDelete_Click(object sender, RoutedEventArgs e) => DeleteSelected();
        private void DeleteSelected()
        {
            TaskItem item = lstTasks.SelectedItem as TaskItem;
            if (item == null)
            {
                MessageBox.Show("Выберите задачу для удаления.",
                    "Ничего не выбрано", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var r = MessageBox.Show(
                "Удалить задачу «" + item.Text + "»?",
                "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r == MessageBoxResult.Yes)
            {
                tasks.Remove(item);
                RefreshStatus();
            }
        }
        // Отметить выполненной
        private void BtnToggle_Click(object sender, RoutedEventArgs e) => ToggleSelected();
        private void ToggleSelected()
        {
            TaskItem item = lstTasks.SelectedItem as TaskItem;
            if (item == null) return;
            item.Done = !item.Done;
            RefreshStatus();
        }
        // Очистить выполненные
        private void BtnClearDone_Click(object sender, RoutedEventArgs e)
        {
            int removed = 0;
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                if (tasks[i].Done)
                {
                    tasks.RemoveAt(i);
                    removed++;
                }
            }
            if (removed == 0)
            {
                MessageBox.Show("Выполненных задач нет.",
                    "Нечего удалять", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            RefreshStatus();
        }
        // Обновить статус
        private void RefreshStatus()
        {
            int total = tasks.Count;
            int done = 0;
            foreach (var t in tasks)
                if (t.Done) done++;
            lblStatus.Text = string.Format(
                "Всего: {0}   |   Выполнено: {1}   |   Осталось: {2}",
                total, done, total - done);
        }
        // Двойной клик по списку
        private void LstTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ToggleSelected();
        }
        // Горячие клавиши в списке
        private void LstTasks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteSelected();
                e.Handled = true;
            }
            else if (e.Key == Key.Space)
            {
                ToggleSelected();
                e.Handled = true;
            }
        }
    }
    // Модель задачи
    public class TaskItem : INotifyPropertyChanged
    {
        private string _text;
        private bool _done;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
                OnPropertyChanged("DisplayText");
            }
        }
        public bool Done
        {
            get { return _done; }
            set
            {
                _done = value;
                OnPropertyChanged("Done");
                OnPropertyChanged("DisplayText");
            }
        }
        public string DisplayText
        {
            get { return (Done ? "+ " : "- ") + Text; }
        }
        public TaskItem(string text)
        {
            _text = text;
            _done = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
    }
}