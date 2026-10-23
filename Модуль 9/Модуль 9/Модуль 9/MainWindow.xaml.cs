using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
namespace Модуль_9
{
    public partial class MainWindow : Window
    {
        private readonly TaskService _service = new TaskService();
        private readonly DispatcherTimer _notifyTimer;
        private DateTime _lastNotifyDate = DateTime.MinValue;
        public MainWindow()
        {
            InitializeComponent();
            lstTasks.ItemsSource = _service.Tasks;
            // Редактирование при двойном клике по тексту — отключим через Ctrl,
            // чтобы не конфликтовать с Toggle. Здесь — только обновление.
            _service.Tasks.CollectionChanged += (s, e) => RefreshStatus();
            RefreshStatus();
            // Таймер уведомлений: раз в минуту проверяем сроки
            _notifyTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            _notifyTimer.Tick += (s, e) => CheckNotifications();
            _notifyTimer.Start();
            // Первичная проверка при запуске
            Loaded += (s, e) => CheckNotifications();
        }
        // Добавление
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
            var priority = (TaskPriority)cmbPriority.SelectedIndex;
            DateTime? due = dpDue.SelectedDate;
            if (_service.Add(txtInput.Text, priority, due))
            {
                txtInput.Clear();
                dpDue.SelectedDate = null;
                cmbPriority.SelectedIndex = 1;
                txtInput.Focus();
                RefreshStatus();
            }
        }
        // Редактирование
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var item = lstTasks.SelectedItem as TaskItem;
            if (item == null)
            {
                MessageBox.Show("Выберите задачу для редактирования.",
                    "Ничего не выбрано", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var dlg = new TaskEditWindow(item);
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                _service.Update(item, dlg.TaskText, dlg.TaskPriority, dlg.TaskDueDate);
                RefreshStatus();
            }
        }
        // Удаление
        private void BtnDelete_Click(object sender, RoutedEventArgs e) => DeleteSelected();
        private void DeleteSelected()
        {
            var item = lstTasks.SelectedItem as TaskItem;
            if (_service.DeleteSelected(item))
                RefreshStatus();
        }
        // Переключение выполнения
        private void BtnToggle_Click(object sender, RoutedEventArgs e) => ToggleSelected();
        private void ToggleSelected()
        {
            var item = lstTasks.SelectedItem as TaskItem;
            if (_service.ToggleSelected(item))
                RefreshStatus();
        }
        // Очистка выполненных
        private void BtnClearDone_Click(object sender, RoutedEventArgs e)
        {
            if (_service.ClearDone())
                RefreshStatus();
        }
        // Проверка сроков вручную
        private void BtnNotify_Click(object sender, RoutedEventArgs e)
        {
            ShowDueSoonNotification(force: true);
        }
        // Уведомления
        private void CheckNotifications()
        {
            if (_lastNotifyDate.Date == DateTime.Today) return;
            _lastNotifyDate = DateTime.Today;
            ShowDueSoonNotification(force: false);
        }
        private void ShowDueSoonNotification(bool force)
        {
            var upcoming = _service.GetUpcoming(2);
            if (upcoming.Count == 0)
            {
                if (force)
                    MessageBox.Show("Нет задач с приближающимся сроком.",
                        "Уведомления", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var sb = new StringBuilder();
            sb.AppendLine("Задачи с приближающимся или истёкшим сроком:");
            sb.AppendLine();
            foreach (var t in upcoming)
            {
                string marker = t.IsOverdue ? "⚠ ПРОСРОЧЕНО" : "⏰ Скоро";
                sb.AppendLine(string.Format("{0}: «{1}» — {2}",
                    marker, t.Text, t.DueDateText));
            }
            MessageBox.Show(sb.ToString(), "Напоминание о сроках",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        // Статус и прогресс
        private void RefreshStatus()
        {
            lblStatus.Text = _service.GetStatusText();
            double p = _service.GetProgress();
            prgProgress.Value = p;
            lblProgressPercent.Text = (p * 100).ToString("0") + "%";
            // Окраска прогресс-бара
            if (p >= 1.0)
                prgProgress.Foreground = new SolidColorBrush(Color.FromRgb(0x43, 0xA0, 0x47));
            else if (p >= 0.5)
                prgProgress.Foreground = new SolidColorBrush(Color.FromRgb(0x1E, 0x88, 0xE5));
            else
                prgProgress.Foreground = new SolidColorBrush(Color.FromRgb(0xFB, 0x8C, 0x00));
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
            else if (e.Key == Key.F2)
            {
                BtnEdit_Click(sender, e);
                e.Handled = true;
            }
        }
        // Двойной клик — переключение выполнения
        private void LstTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Если попали по пустому месту — игнорируем
            var item = ItemsControl.ContainerFromElement(lstTasks, e.OriginalSource as DependencyObject)
                       as ListBoxItem;
            if (item == null) return;
            ToggleSelected();
        }
    }
}