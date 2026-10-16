using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using WpfColor = System.Windows.Media.Color;
using WpfColors = System.Windows.Media.Colors;
namespace TextEditorApp
{
    public class EditorService
    {
        private readonly RichTextBox _richTextBox;
        public EditorService(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
        }
        // Заменить содержимое редактора новым текстом
        public void SetText(string text)
        {
            _richTextBox.Document.Blocks.Clear();
            _richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }
        // Получить весь текст из редактора
        public string GetText()
        {
            var range = new TextRange(
                _richTextBox.Document.ContentStart,
                _richTextBox.Document.ContentEnd);
            return range.Text;
        }
        // Применить параметры шрифта
        public void ApplyFont(FontFamily family, double size, FontWeight weight,
                              FontStyle style, WpfColor color)
        {
            _richTextBox.FontFamily = family;
            _richTextBox.FontSize = size;
            _richTextBox.FontWeight = weight;
            _richTextBox.FontStyle = style;
            _richTextBox.Foreground = new SolidColorBrush(color);
        }
        // Текущие параметры шрифта (для диалога)
        public FontFamily CurrentFontFamily => _richTextBox.FontFamily;
        public double CurrentFontSize => _richTextBox.FontSize;
        public FontWeight CurrentFontWeight => _richTextBox.FontWeight;
        public FontStyle CurrentFontStyle => _richTextBox.FontStyle;
        public WpfColor CurrentColor =>
            (_richTextBox.Foreground as SolidColorBrush)?.Color ?? WpfColors.Black;
    }
}