using System.Windows;

namespace Модуль_6_Задание_1
{
    public partial class MainWindow : Window
    {
        private readonly DB db = new DB();
        public MainWindow()
        {
            InitializeComponent();
            ShowResult(db.GetAll()); // первичная загрузка
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var result = db.Add(NameTextBox.Text, DolwnostTextBox.Text);
            ShowResult(result);

            if (result.Success)
            {
                NameTextBox.Clear();
                DolwnostTextBox.Clear();
                ShowResult(db.GetAll());
            }
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = db.Remove(RemoveNameTextBox.Text);
            ShowResult(result);

            if (result.Success)
            {
                RemoveNameTextBox.Clear();
                ShowResult(db.GetAll());
            }
        }
        private void PrintAllButton_Click(object sender, RoutedEventArgs e)
        {
            ShowResult(db.GetAll());
        }
        // Единственный вспомогательный метод — только вывод в UI
        private void ShowResult(OperationResult result)
        {
            StatusTextBlock.Text = result.Message;
            if (result.Items.Count > 0 || result.Success)
            {
                EmployeesListBox.Items.Clear();
                foreach (var item in result.Items)
                    EmployeesListBox.Items.Add(item);
            }
        }
    }
}