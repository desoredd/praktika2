using System.Windows;
using WpfColors = System.Windows.Media.Colors;

namespace TextEditorApp
{
    public partial class MainWindow : Window
    {
        private readonly FileService _fileService = new FileService();
        private EditorService _editorService;
        public MainWindow()
        {
            InitializeComponent();
            _editorService = new EditorService(richTextBox);
        }
        // Открыть
        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (_editorService == null) return;

            string text = _fileService.OpenFile();
            if (text == null) return;

            _editorService.SetText(text);

            MessageBox.Show("Файл успешно прочитан!", "Успех",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // Сохранить
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_editorService == null) return;

            string text = _editorService.GetText();

            if (_fileService.SaveFile(text))
            {
                MessageBox.Show("Файл успешно сохранён!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        // Шрифт
        private void BtnFont_Click(object sender, RoutedEventArgs e)
        {
            if (_editorService == null) return;
            var dialog = new FontDialogWindow(
                _editorService.CurrentFontFamily,
                _editorService.CurrentFontSize,
                _editorService.CurrentFontWeight,
                _editorService.CurrentFontStyle,
                _editorService.CurrentColor);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                _editorService.ApplyFont(
                    dialog.SelectedFontFamily,
                    dialog.SelectedFontSize,
                    dialog.SelectedFontWeight,
                    dialog.SelectedFontStyle,
                    dialog.SelectedColor);
            }
        }
    }
}