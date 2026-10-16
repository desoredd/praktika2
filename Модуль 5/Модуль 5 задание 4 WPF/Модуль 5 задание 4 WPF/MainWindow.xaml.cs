using System.Windows;
using System.Windows.Input;
namespace Модуль_5_Задание_4
{
    public partial class MainWindow : Window
    {
        private ImageViewerService _viewer;
        public MainWindow()
        {
            InitializeComponent();
            _viewer = new ImageViewerService(drawCanvas, imgDisplay, canvasBorder);
            canvasBorder.PreviewMouseWheel += Canvas_MouseWheel;
        }
        // Меню: Открыть / Закрыть / Выход
        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            if (_viewer == null) return;
            if (_viewer.OpenImage())
                UpdateStatus();
        }
        private void CloseItem_Click(object sender, RoutedEventArgs e)
        {
            if (_viewer == null) return;
            _viewer.CloseImage();
            UpdateStatus();
        }
        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        // Размер холста
        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_viewer == null) return;
            if (_viewer.Image != null) _viewer.Redraw();
        }
        //Масштаб
        private void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (_viewer == null) return;
            _viewer.ZoomIn();
            UpdateStatus();
        }
        private void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (_viewer == null) return;
            _viewer.ZoomOut();
            UpdateStatus();
        }
        private void BtnZoomReset_Click(object sender, RoutedEventArgs e)
        {
            if (_viewer == null) return;
            _viewer.ZoomReset();
            UpdateStatus();
        }
        private void BtnFit_Click(object sender, RoutedEventArgs e)
        {
            if (_viewer == null) return;
            _viewer.FitToWindow();
            UpdateStatus();
        }
        // Колесо мыши
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_viewer == null) return;
            _viewer.OnMouseWheel(e);
            UpdateStatus();
        }
        // Перетаскивание 
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewer == null) return;
            _viewer.OnMouseDown(e);
            if (e.Handled) Cursor = Cursors.SizeAll;
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_viewer == null) return;
            _viewer.OnMouseMove(e);
        }
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_viewer == null) return;
            _viewer.OnMouseUp(e);
            Cursor = Cursors.Arrow;
        }
        // Статус 
        private void UpdateStatus()
        {
            lblInfo.Text = _viewer.GetStatusText();
        }
    }
}