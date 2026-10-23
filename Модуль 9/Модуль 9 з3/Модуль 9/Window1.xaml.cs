using System;
using System.Windows;
namespace Модуль_9
{
    public partial class TaskEditWindow : Window
    {
        public string TaskText { get; private set; }
        public TaskPriority TaskPriority { get; private set; }
        public DateTime? TaskDueDate { get; private set; }
        public TaskEditWindow(TaskItem item)
        {
            InitializeComponent();

            txtText.Text = item.Text;
            cmbPriority.SelectedIndex = (int)item.Priority;
            dpDue.SelectedDate = item.DueDate;
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            string text = (txtText.Text ?? "").Trim();
            if (text.Length == 0)
            {
                MessageBox.Show("Текст задачи не может быть пустым.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            TaskText = text;
            TaskPriority = (TaskPriority)cmbPriority.SelectedIndex;
            TaskDueDate = dpDue.SelectedDate;
            DialogResult = true;
        }
    }
}