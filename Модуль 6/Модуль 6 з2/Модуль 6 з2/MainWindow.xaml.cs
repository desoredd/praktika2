using System.Windows;
namespace Модуль_6_Задание_2
{
    public partial class MainWindow : Window
    {
        private readonly DB _db = new DB();
        public MainWindow()
        {
            InitializeComponent();
            ShowResult(_db.GetAll()); // первичная загрузка
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _db.Add(NameTextBox.Text, TextTextBox.Text);
            ShowResult(result);
            if (result.Success)
            {
                NameTextBox.Clear();
                TextTextBox.Clear();
                ShowResult(_db.GetAll());
            }
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _db.Remove(RemoveNameTextBox.Text);
            ShowResult(result);

            if (result.Success)
            {
                RemoveNameTextBox.Clear();
                ShowResult(_db.GetAll());
            }
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _db.Update(UpdateNameTextBox.Text, UpdateTextTextBox.Text);
            ShowResult(result);
            if (result.Success)
            {
                UpdateNameTextBox.Clear();
                UpdateTextTextBox.Clear();
                ShowResult(_db.GetAll());
            }
        }
        private void PrintAllButton_Click(object sender, RoutedEventArgs e)
        {
            ShowResult(_db.GetAll());
        }
        // Единственный вспомогательный метод - вывод в UI
        private void ShowResult(OperationResult result)
        {
            StatusTextBlock.Text = result.Message;
            if (result.Success)
            {
                TasksListBox.Items.Clear();
                foreach (var item in result.Items)
                    TasksListBox.Items.Add(item);
            }
        }
    }
}