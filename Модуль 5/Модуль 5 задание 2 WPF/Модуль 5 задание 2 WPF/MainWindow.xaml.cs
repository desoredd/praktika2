using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Win32;

using WpfColor = System.Windows.Media.Color;
using WpfColors = System.Windows.Media.Colors;

namespace TextEditorApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // ---------- Открыть файл ----------
        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Выберите текстовый файл для открытия"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // В WPF RichTextBox — используем TextRange для загрузки текста
                    string text = File.ReadAllText(openFileDialog.FileName);
                    richTextBox.Document.Blocks.Clear();
                    richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));

                    MessageBox.Show("Файл успешно прочитан!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ---------- Сохранить файл ----------
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Сохранить файл как...",
                DefaultExt = "txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Получаем весь текст из RichTextBox через TextRange
                    TextRange range = new TextRange(
                        richTextBox.Document.ContentStart,
                        richTextBox.Document.ContentEnd);

                    File.WriteAllText(saveFileDialog.FileName, range.Text);

                    MessageBox.Show("Файл успешно сохранён!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ---------- Настройка шрифта ----------
        private void BtnFont_Click(object sender, RoutedEventArgs e)
        {
            // В WPF нет встроенного FontDialog.
            // Используем простой самодельный диалог выбора шрифта и цвета.
            var dialog = new FontDialogWindow(
                richTextBox.FontFamily,
                richTextBox.FontSize,
                richTextBox.FontWeight,
                richTextBox.FontStyle,
                (richTextBox.Foreground as SolidColorBrush)?.Color ?? WpfColors.Black);

            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                richTextBox.FontFamily = dialog.SelectedFontFamily;
                richTextBox.FontSize = dialog.SelectedFontSize;
                richTextBox.FontWeight = dialog.SelectedFontWeight;
                richTextBox.FontStyle = dialog.SelectedFontStyle;
                richTextBox.Foreground = new SolidColorBrush(dialog.SelectedColor);
            }
        }
    }
}