using System.Windows;
using System.Windows.Controls;
namespace Модуль_6_Задание_4
{
    public partial class MainWindow : Window
    {
        private readonly DB _db = new DB();
        public MainWindow()
        {
            InitializeComponent();
            ShowResult(_db.GetAll());
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string type = ((ComboBoxItem)AddTypeComboBox.SelectedItem).Tag.ToString();
            var result = _db.Add(type, AddCategoryTextBox.Text, AddAmountTextBox.Text, AddDateTextBox.Text, AddCommentTextBox.Text);
            ShowResult(result);
            if (result.Success)
            {
                AddCategoryTextBox.Clear();
                AddAmountTextBox.Clear();
                AddDateTextBox.Clear();
                AddCommentTextBox.Clear();
                ShowResult(_db.GetAll());
            }
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string type = ((ComboBoxItem)EditTypeComboBox.SelectedItem).Tag.ToString();
            var result = _db.Update(EditIdTextBox.Text, type, EditCategoryTextBox.Text, EditAmountTextBox.Text, EditDateTextBox.Text, EditCommentTextBox.Text);
            ShowResult(result);
            if (result.Success)
            {
                EditIdTextBox.Clear();
                EditCategoryTextBox.Clear();
                EditAmountTextBox.Clear();
                EditDateTextBox.Clear();
                EditCommentTextBox.Clear();
                ShowResult(_db.GetAll());
            }
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _db.Remove(EditIdTextBox.Text);
            ShowResult(result);
            if (result.Success)
            {
                EditIdTextBox.Clear();
                ShowResult(_db.GetAll());
            }
        }
        private void PrintAllButton_Click(object sender, RoutedEventArgs e)
        {
            ShowResult(_db.GetAll());
        }
        private void SummaryButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _db.GetSummary();
            // Для итогов просто показываем сообщение в статусе, список не трогаем
            StatusTextBlock.Text = result.Message;
        }
        // Обработчики фильтров
        private void FilterByType_Click(object sender, RoutedEventArgs e)
        {
            string type = ((ComboBoxItem)FilterTypeComboBox.SelectedItem).Tag.ToString();
            if (string.IsNullOrEmpty(type))
                ShowResult(_db.GetAll());
            else
                ShowResult(_db.FilterByType(type));
        }
        private void FilterByCategory_Click(object sender, RoutedEventArgs e)
        {
            ShowResult(_db.FilterByCategory(FilterCategoryTextBox.Text));
        }
        private void FilterByDate_Click(object sender, RoutedEventArgs e)
        {
            ShowResult(_db.FilterByDate(FilterDateTextBox.Text));
        }
        // Единственный вспомогательный метод — только вывод в UI
        private void ShowResult(OperationResult result)
        {
            StatusTextBlock.Text = result.Message;

            if (result.Success)
            {
                TransactionsListBox.Items.Clear();
                foreach (var item in result.Items)
                    TransactionsListBox.Items.Add(item);
            }
        }
    }
}