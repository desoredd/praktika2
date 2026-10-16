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
    public class ImageViewerService
    {
        private readonly Canvas _canvas;
        private readonly Image _imageControl;
        private readonly Border _canvasBorder;
        public BitmapImage Image { get; private set; }
        public string CurrentFile { get; private set; }
        public double Zoom { get; private set; } = 1.0;
        public Point Offset { get; private set; } = new Point(0, 0);
        public const double ZoomMin = 0.05;
        public const double ZoomMax = 20.0;
        public const double ZoomStep = 1.2;
        private bool _isPanning;
        private Point _panStart;
        public ImageViewerService(Canvas canvas, Image imageControl, Border canvasBorder)
        {
            _canvas = canvas;
            _imageControl = imageControl;
            _canvasBorder = canvasBorder;
        }
        // Открытие / закрытие
        // Возвращает true, если изображение успешно открыто
        public bool OpenImage()
        {
            var dlg = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Все файлы|*.*"
            };
            if (dlg.ShowDialog() != true) return false;
            try
            {
                byte[] data = File.ReadAllBytes(dlg.FileName);
                using (var ms = new MemoryStream(data))
                {
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.StreamSource = ms;
                    bmp.EndInit();
                    bmp.Freeze();
                    Image = bmp;
                }
                CurrentFile = dlg.FileName;
                FitToWindow();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть файл:\n" + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public void CloseImage()
        {
            Image = null;
            CurrentFile = null;
            Zoom = 1.0;
            Offset = new Point(0, 0);
            _imageControl.Source = null;
        }
        // Отрисовка
        public void Redraw()
        {
            if (Image == null)
            {
                _imageControl.Source = null;
                return;
            }
            _imageControl.Source = Image;
            double w = Image.PixelWidth * Zoom;
            double h = Image.PixelHeight * Zoom;
            double cx = _canvas.ActualWidth / 2.0 + Offset.X;
            double cy = _canvas.ActualHeight / 2.0 + Offset.Y;
            _imageControl.Width = w;
            _imageControl.Height = h;
            Canvas.SetLeft(_imageControl, cx - w / 2.0);
            Canvas.SetTop(_imageControl, cy - h / 2.0);
            RenderOptions.SetBitmapScalingMode(_imageControl,
                Zoom >= 1.0 ? BitmapScalingMode.NearestNeighbor
                            : BitmapScalingMode.HighQuality);
        }
        // Масштаб 
        public void ZoomIn() => ZoomAtCenter(ZoomStep);
        public void ZoomOut() => ZoomAtCenter(1.0 / ZoomStep);
        public void ZoomReset()
        {
            Zoom = 1.0;
            Offset = new Point(0, 0);
            Redraw();
        }
        public void ZoomAtCenter(double factor)
        {
            if (Image == null) return;
            double newZoom = Math.Max(ZoomMin, Math.Min(ZoomMax, Zoom * factor));
            double k = newZoom / Zoom;
            Offset = new Point(Offset.X * k, Offset.Y * k);
            Zoom = newZoom;
            Redraw();
        }
        public void ZoomAtPoint(Point mouse, double factor)
        {
            if (Image == null) return;
            double newZoom = Math.Max(ZoomMin, Math.Min(ZoomMax, Zoom * factor));
            double k = newZoom / Zoom;
            double cx = _canvas.ActualWidth / 2.0;
            double cy = _canvas.ActualHeight / 2.0;
            double dx = mouse.X - cx - Offset.X;
            double dy = mouse.Y - cy - Offset.Y;
            Offset = new Point(
                Offset.X - dx * (k - 1),
                Offset.Y - dy * (k - 1));
            Zoom = newZoom;
            Redraw();
        }
        public void FitToWindow()
        {
            if (Image == null) return;
            double sw = _canvas.ActualWidth / Image.PixelWidth;
            double sh = _canvas.ActualHeight / Image.PixelHeight;
            Zoom = Math.Min(sw, sh) * 0.98;
            Offset = new Point(0, 0);
            Redraw();
        }
        // Колесо мыши
        public void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (Image == null) return;
            double factor = e.Delta > 0 ? ZoomStep : 1.0 / ZoomStep;
            Point mouse = e.GetPosition(_canvas);
            ZoomAtPoint(mouse, factor);
            e.Handled = true;
        }
        // Перетаскивание
        public void OnMouseDown(MouseButtonEventArgs e)
        {
            if (Image == null) return;
            if (e.LeftButton != MouseButtonState.Pressed) return;
            _isPanning = true;
            _panStart = e.GetPosition(_canvas);
            _canvas.CaptureMouse();
            e.Handled = true;
        }
        public void OnMouseMove(MouseEventArgs e)
        {
            if (!_isPanning) return;
            Point pt = e.GetPosition(_canvas);
            Offset = new Point(
                Offset.X + (pt.X - _panStart.X),
                Offset.Y + (pt.Y - _panStart.Y));
            _panStart = pt;
            Redraw();
        }
        public void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Released) return;
            _isPanning = false;
            _canvas.ReleaseMouseCapture();
        }
        // Строка статуса
        public string GetStatusText()
        {
            if (Image == null)
                return "Изображение не загружено";
            string name = CurrentFile != null
                ? Path.GetFileName(CurrentFile)
                : "(без имени)";
            return string.Format("{0}   |   {1}×{2} px   |   Масштаб: {3:0}%",
                name, Image.PixelWidth, Image.PixelHeight, Zoom * 100);
        }
    }
}