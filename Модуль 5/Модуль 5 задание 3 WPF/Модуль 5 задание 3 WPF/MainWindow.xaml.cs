using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Модуль_5_задание_3
{
    public partial class MainWindow : Window
    {
        private readonly TaskService _service = new TaskService();

        public MainWindow()
        {
            InitializeComponent();

            lstTasks.ItemsSource = _service.Tasks;
            RefreshStatus();
        }
        // Добавление
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTask();
        }
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
            if (_service.Add(txtInput.Text))
            {
                txtInput.Clear();
                txtInput.Focus();
                RefreshStatus();
            }
        }
        // Удаление
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelected();
        }
        private void DeleteSelected()
        {
            var item = lstTasks.SelectedItem as TaskItem;
            if (_service.DeleteSelected(item))
                RefreshStatus();
        }
        // Отметить выполненной
        private void BtnToggle_Click(object sender, RoutedEventArgs e)
        {
            ToggleSelected();
        }
        private void ToggleSelected()
        {
            var item = lstTasks.SelectedItem as TaskItem;
            if (_service.ToggleSelected(item))
                RefreshStatus();
        }
        // Очистить выполненные

        private void BtnClearDone_Click(object sender, RoutedEventArgs e)
        {
            if (_service.ClearDone())
                RefreshStatus();
        }
        // Статус
        private void RefreshStatus()
        {
            lblStatus.Text = _service.GetStatusText();
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
}