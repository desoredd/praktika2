using System.Windows;
namespace Модуль_6_Задание_3
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
            var result = _db.Add(AddNameTextBox.Text, AddAuthorTextBox.Text, AddGenreTextBox.Text, AddVpTextBox.Text);
            ShowResult(result);
            if (result.Success)
            {
                AddNameTextBox.Clear();
                AddAuthorTextBox.Clear();
                AddGenreTextBox.Clear();
                AddVpTextBox.Clear();
                ShowResult(_db.GetAll());
            }
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _db.Update(EditNameTextBox.Text, EditAuthorTextBox.Text, EditGenreTextBox.Text, EditVpTextBox.Text);
            ShowResult(result);
            if (result.Success)
            {
                EditNameTextBox.Clear();
                EditAuthorTextBox.Clear();
                EditGenreTextBox.Clear();
                EditVpTextBox.Clear();
                ShowResult(_db.GetAll());
            }
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _db.Remove(EditNameTextBox.Text);
            ShowResult(result);
            if (result.Success)
            {
                EditNameTextBox.Clear();
                ShowResult(_db.GetAll());
            }
        }
        private void PrintAllButton_Click(object sender, RoutedEventArgs e)
        {
            ShowResult(_db.GetAll());
        }
        // Обработчики фильтров
        private void FilterByName_Click(object sender, RoutedEventArgs e) =>
            ShowResult(_db.FilterByName(FilterNameTextBox.Text));
        private void FilterByAuthor_Click(object sender, RoutedEventArgs e) =>
            ShowResult(_db.FilterByAuthor(FilterAuthorTextBox.Text));
        private void FilterByGenre_Click(object sender, RoutedEventArgs e) =>
            ShowResult(_db.FilterByGenre(FilterGenreTextBox.Text));
        private void FilterByVp_Click(object sender, RoutedEventArgs e) =>
            ShowResult(_db.FilterByVp(FilterVpTextBox.Text));
        // Единственный вспомогательный метод — только вывод в UI
        private void ShowResult(OperationResult result)
        {
            StatusTextBlock.Text = result.Message;
            if (result.Success)
            {
                BooksListBox.Items.Clear();
                foreach (var item in result.Items)
                    BooksListBox.Items.Add(item);
            }
        }
    }
}