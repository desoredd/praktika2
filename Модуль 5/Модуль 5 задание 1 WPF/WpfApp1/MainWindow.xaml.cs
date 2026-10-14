using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfColor = System.Windows.Media.Color;
using WpfColors = System.Windows.Media.Colors;
namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private ShapeDrawer _drawer;
        public MainWindow()
        {
            InitializeComponent();
            _drawer = new ShapeDrawer(drawCanvas);
        }
        // Мышь на холсте
        private void DrawCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_drawer == null) return;
            _drawer.OnMouseDown(e, e.GetPosition(drawCanvas));
        }
        private void DrawCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_drawer == null) return;
            _drawer.OnMouseMove(e, e.GetPosition(drawCanvas));
        }
        private void DrawCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_drawer == null) return;
            _drawer.OnMouseUp(e, e.GetPosition(drawCanvas));
        }
        // Инструменты
        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            if (_drawer == null) return;
            _drawer.CurrentTool = ShapeType.Line;
        }
        private void CircleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_drawer == null) return;
            _drawer.CurrentTool = ShapeType.Circle;
        }
        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_drawer == null) return;
            _drawer.CurrentTool = ShapeType.Rectangle;
        }
        private void FreehandButton_Click(object sender, RoutedEventArgs e)
        {
            if (_drawer == null) return;
            _drawer.CurrentTool = ShapeType.Freehand;
        }
        // Очистка
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (_drawer == null) return;
            _drawer.Clear();
        }
        // Цвет
        private void CmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_drawer == null) return;

            if (cmbColor.SelectedItem is ComboBoxItem item && item.Tag is string tag)
            {
                try
                {
                    _drawer.CurrentColor = (WpfColor)ColorConverter.ConvertFromString(tag);
                }
                catch
                {
                    _drawer.CurrentColor = WpfColors.Black;
                }
            }
        }
        // Толщина
        private void NumThickness_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_drawer == null) return;

            if (double.TryParse(numThickness.Text, out double t))
            {
                if (t < 1) t = 1;
                _drawer.CurrentThickness = t;
            }
        }
    }
}