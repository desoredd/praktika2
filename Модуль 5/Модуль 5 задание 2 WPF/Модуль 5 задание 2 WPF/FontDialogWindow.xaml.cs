using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfColor = System.Windows.Media.Color;
using WpfColors = System.Windows.Media.Colors;
namespace TextEditorApp
{
    public partial class FontDialogWindow : Window
    {
        public FontFamily SelectedFontFamily { get; private set; }
        public double SelectedFontSize { get; private set; }
        public FontWeight SelectedFontWeight { get; private set; }
        public FontStyle SelectedFontStyle { get; private set; }
        public WpfColor SelectedColor { get; private set; }
        public FontDialogWindow(FontFamily family, double size,
                                FontWeight weight, FontStyle style, WpfColor color)
        {
            InitializeComponent();
            SelectedFontFamily = family;
            SelectedFontSize = size;
            SelectedFontWeight = weight;
            SelectedFontStyle = style;
            SelectedColor = color;
            foreach (var ff in Fonts.SystemFontFamilies)
                cmbFont.Items.Add(ff.Source);
            double[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 24, 28, 32, 36, 48, 72 };
            foreach (var s in sizes)
                cmbSize.Items.Add(s);
            cmbFont.SelectedItem = family.Source;
            cmbSize.Text = size.ToString();
            if (weight == FontWeights.Bold && style == FontStyles.Italic)
                cmbStyle.SelectedIndex = 3;
            else if (weight == FontWeights.Bold)
                cmbStyle.SelectedIndex = 2;
            else if (style == FontStyles.Italic)
                cmbStyle.SelectedIndex = 1;
            else
                cmbStyle.SelectedIndex = 0;
            cmbColor.SelectedIndex = 0;
            foreach (ComboBoxItem item in cmbColor.Items)
            {
                if (item.Tag is string tag)
                {
                    try
                    {
                        var c = (WpfColor)ColorConverter.ConvertFromString(tag);
                        if (c == color) { cmbColor.SelectedItem = item; break; }
                    }
                    catch { }
                }
            }

            UpdatePreview();
        }
        private void CmbFont_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFont.SelectedItem is string name)
                SelectedFontFamily = new FontFamily(name);
            UpdatePreview();
        }
        private void CmbSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (double.TryParse(cmbSize.Text, out double s) && s > 0)
                SelectedFontSize = s;
            UpdatePreview();
        }
        private void CmbStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbStyle.SelectedIndex)
            {
                case 1: SelectedFontStyle = FontStyles.Italic; SelectedFontWeight = FontWeights.Normal; break;
                case 2: SelectedFontStyle = FontStyles.Normal; SelectedFontWeight = FontWeights.Bold; break;
                case 3: SelectedFontStyle = FontStyles.Italic; SelectedFontWeight = FontWeights.Bold; break;
                default: SelectedFontStyle = FontStyles.Normal; SelectedFontWeight = FontWeights.Normal; break;
            }
            UpdatePreview();
        }
        private void CmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbColor.SelectedItem is ComboBoxItem item && item.Tag is string tag)
            {
                try { SelectedColor = (WpfColor)ColorConverter.ConvertFromString(tag); }
                catch { SelectedColor = WpfColors.Black; }
            }
            UpdatePreview();
        }
        private void UpdatePreview()
        {
            if (txtPreview == null) return;
            txtPreview.FontFamily = SelectedFontFamily;
            txtPreview.FontSize = SelectedFontSize;
            txtPreview.FontWeight = SelectedFontWeight;
            txtPreview.FontStyle = SelectedFontStyle;
            txtPreview.Foreground = new SolidColorBrush(SelectedColor);
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}