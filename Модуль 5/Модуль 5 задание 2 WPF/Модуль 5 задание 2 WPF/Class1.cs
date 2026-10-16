using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Win32;

namespace TextEditorApp
{
    public class FileService
    {
        // Открыть файл и вернуть его содержимое, либо null при отмене/ошибке
        public string OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Выберите текстовый файл для открытия"
            };

            if (openFileDialog.ShowDialog() != true)
                return null;

            try
            {
                return File.ReadAllText(openFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        // Сохранить текст в файл. Возвращает true при успехе
        public bool SaveFile(string text)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Сохранить файл как...",
                DefaultExt = "txt"
            };

            if (saveFileDialog.ShowDialog() != true)
                return false;
            try
            {
                File.WriteAllText(saveFileDialog.FileName, text);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}