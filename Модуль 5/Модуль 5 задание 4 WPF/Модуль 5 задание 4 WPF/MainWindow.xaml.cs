using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace Модуль_5_Задание_4
{
    public partial class MainWindow : Window
    {
        private BitmapImage image = null;
        private double zoom = 1.0;          // 1.0 = 100%
        private Point offset = new Point(0, 0); // смещение относительно центра
        private const double ZoomMin = 0.05;
        private const double ZoomMax = 20.0;
        private const double ZoomStep = 1.2;
        private bool isPanning = false;
        private Point panStart;
        private string currentFile = null;
        public MainWindow()
        {
            InitializeComponent();
            // Обработка колеса мыши на всём окне
            canvasBorder.PreviewMouseWheel += Canvas_MouseWheel;
        }
        // Открытие / закрытие
        private void OpenItem_Click(object sender, RoutedEventArgs e) => OpenImage();
        private void OpenImage()
        {
            var dlg = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Все файлы|*.*"
            };
            if (dlg.ShowDialog(this) != true) return;
            try
            {
                // Читаем через поток, чтобы не блокировать файл
                byte[] data = File.ReadAllBytes(dlg.FileName);
                using (var ms = new MemoryStream(data))
                {
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.StreamSource = ms;
                    bmp.EndInit();
                    bmp.Freeze();
                    image = bmp;
                }
                currentFile = dlg.FileName;
                FitToWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть файл:\n" + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CloseItem_Click(object sender, RoutedEventArgs e) => CloseImage();
        private void CloseImage()
        {
            image = null;
            currentFile = null;
            zoom = 1.0;
            offset = new Point(0, 0);
            imgDisplay.Source = null;
            UpdateStatus();
        }
        private void ExitItem_Click(object sender, RoutedEventArgs e) => Close();
        // Отрисовка
        private void Redraw()
        {
            if (image == null)
            {
                imgDisplay.Source = null;
                return;
            }
            imgDisplay.Source = image;
            double w = image.PixelWidth * zoom;
            double h = image.PixelHeight * zoom;
            // центр холста + смещение
            double cx = drawCanvas.ActualWidth / 2.0 + offset.X;
            double cy = drawCanvas.ActualHeight / 2.0 + offset.Y;
            imgDisplay.Width = w;
            imgDisplay.Height = h;
            Canvas.SetLeft(imgDisplay, cx - w / 2.0);
            Canvas.SetTop(imgDisplay, cy - h / 2.0);
            // Режим интерполяции: NearestNeighbor при увеличении (пиксель в пиксель),
            // HighQualityBicubic при уменьшении
            RenderOptions.SetBitmapScalingMode(imgDisplay,
                zoom >= 1.0 ? BitmapScalingMode.NearestNeighbor
                            : BitmapScalingMode.HighQuality);
        }
        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (image != null) Redraw();
        }
        // Масштаб
        private void BtnZoomIn_Click(object sender, RoutedEventArgs e) => ZoomAtCenter(ZoomStep);
        private void BtnZoomOut_Click(object sender, RoutedEventArgs e) => ZoomAtCenter(1.0 / ZoomStep);
        private void BtnZoomReset_Click(object sender, RoutedEventArgs e)
        {
            zoom = 1.0;
            offset = new Point(0, 0);
            Redraw();
            UpdateStatus();
        }
        private void BtnFit_Click(object sender, RoutedEventArgs e) => FitToWindow();
        private void ZoomAtCenter(double factor)
        {
            if (image == null) return;
            double newZoom = Math.Max(ZoomMin, Math.Min(ZoomMax, zoom * factor));
            double k = newZoom / zoom;
            offset = new Point(offset.X * k, offset.Y * k);
            zoom = newZoom;
            Redraw();
            UpdateStatus();
        }
        private void ZoomAtPoint(Point mouse, double factor)
        {
            if (image == null) return;
            double newZoom = Math.Max(ZoomMin, Math.Min(ZoomMax, zoom * factor));
            double k = newZoom / zoom;
            double cx = drawCanvas.ActualWidth / 2.0;
            double cy = drawCanvas.ActualHeight / 2.0;
            double dx = mouse.X - cx - offset.X;
            double dy = mouse.Y - cy - offset.Y;
            // Точка под курсором должна остаться на месте
            offset = new Point(
                offset.X - dx * (k - 1),
                offset.Y - dy * (k - 1));
            zoom = newZoom;
            Redraw();
            UpdateStatus();
        }
        private void FitToWindow()
        {
            if (image == null) return;
            double sw = drawCanvas.ActualWidth / image.PixelWidth;
            double sh = drawCanvas.ActualHeight / image.PixelHeight;
            zoom = Math.Min(sw, sh) * 0.98;   // отступ
            offset = new Point(0, 0);
            Redraw();
            UpdateStatus();
        }
        // Колесо мыши
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (image == null) return;
            double factor = e.Delta > 0 ? ZoomStep : 1.0 / ZoomStep;
            Point mouse = e.GetPosition(drawCanvas);
            ZoomAtPoint(mouse, factor);
            e.Handled = true;
        }
        // Перетаскивание
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (image == null) return;
            if (e.LeftButton != MouseButtonState.Pressed) return;
            isPanning = true;
            panStart = e.GetPosition(drawCanvas);
            drawCanvas.CaptureMouse();
            Cursor = Cursors.SizeAll;
            e.Handled = true;
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPanning) return;
            Point pt = e.GetPosition(drawCanvas);
            offset = new Point(
                offset.X + (pt.X - panStart.X),
                offset.Y + (pt.Y - panStart.Y));
            panStart = pt;
            Redraw();
        }
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Released) return;
            isPanning = false;
            drawCanvas.ReleaseMouseCapture();
            Cursor = Cursors.Arrow;
        }
        // Клавиатура
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (image == null) return;
            bool ctrl = Keyboard.Modifiers == ModifierKeys.Control;
            if (ctrl && (e.Key == Key.OemPlus || e.Key == Key.Add))
            {
                ZoomAtCenter(ZoomStep);
                e.Handled = true;
            }
            else if (ctrl && (e.Key == Key.OemMinus || e.Key == Key.Subtract))
            {
                ZoomAtCenter(1.0 / ZoomStep);
                e.Handled = true;
            }
            else if (ctrl && e.Key == Key.D0)
            {
                zoom = 1.0;
                offset = new Point(0, 0);
                Redraw();
                UpdateStatus();
                e.Handled = true;
            }
            else if (ctrl && e.Key == Key.F)
            {
                FitToWindow();
                e.Handled = true;
            }
            else if (ctrl && e.Key == Key.O)
            {
                OpenImage();
                e.Handled = true;
            }
        }
        // Статус
        private void UpdateStatus()
        {
            if (image == null)
            {
                lblInfo.Text = "Изображение не загружено";
                return;
            }
            string name = currentFile != null
                ? Path.GetFileName(currentFile)
                : "(без имени)";
            lblInfo.Text = string.Format("{0}   |   {1}×{2} px   |   Масштаб: {3:0}%",
                name, image.PixelWidth, image.PixelHeight, zoom * 100);
        }
    }
}