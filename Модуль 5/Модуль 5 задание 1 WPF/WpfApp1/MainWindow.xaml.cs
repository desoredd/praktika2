using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfColor = System.Windows.Media.Color;
using WpfColors = System.Windows.Media.Colors;
using WpfPoint = System.Windows.Point;
using WpfRect = System.Windows.Rect;
namespace Модуль_5_Задание_1
{
    public partial class MainWindow : Window
    {
        private List<Shape> shapes = new List<Shape>();
        private Shape current = null;
        private ShapeType currentType = ShapeType.Line;
        private WpfColor currentColor = WpfColors.Black;
        private double currentThickness = 2;
        // Отдельный Shape-объект для "предпросмотра", чтобы не путать с shapes
        private System.Windows.Shapes.Shape previewShape = null;
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += MainWindow_KeyDown;
        }
        // инструменты
        private void BtnLine_Click(object sender, RoutedEventArgs e) => currentType = ShapeType.Line;
        private void BtnCircle_Click(object sender, RoutedEventArgs e) => currentType = ShapeType.Circle;
        private void BtnRectangle_Click(object sender, RoutedEventArgs e) => currentType = ShapeType.Rectangle;
        private void BtnFreehand_Click(object sender, RoutedEventArgs e) => currentType = ShapeType.Freehand;
        private void CmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbColor.SelectedItem is ComboBoxItem item && item.Tag is string colorName)
            {
                try { currentColor = (WpfColor)ColorConverter.ConvertFromString(colorName); }
                catch { currentColor = WpfColors.Black; }
            }
        }
        private void NumThickness_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(numThickness.Text, out double val))
            {
                if (val < 1) val = 1;
                if (val > 20) val = 20;
                currentThickness = val;
            }
        }
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            shapes.Clear();
            drawCanvas.Children.Clear();
            previewShape = null;
        }
        // МЫШЬ 
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            WpfPoint pt = e.GetPosition(drawCanvas);
            current = new Shape
            {
                Type = currentType,
                Start = pt,
                End = pt,
                Color = currentColor,
                Thickness = currentThickness
            };
            if (currentType == ShapeType.Freehand)
                current.Points.Add(pt);
            // чтобы MouseMove и MouseUp приходили сюда, даже если курсор вышел за пределы
            drawCanvas.CaptureMouse();
            drawCanvas.Focus();
            ShowPreview();
            e.Handled = true;
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (current == null) return;
            WpfPoint pt = e.GetPosition(drawCanvas);
            if (current.Type == ShapeType.Freehand)
                current.Points.Add(pt);
            else
                current.End = pt;
            ShowPreview();
        }
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (current == null) return;
            WpfPoint pt = e.GetPosition(drawCanvas);
            if (current.Type == ShapeType.Freehand)
            {
                current.Points.Add(pt);
                if (current.Points.Count > 1)
                    CommitCurrent();
            }
            else
            {
                current.End = pt;
                if (current.Start != current.End)
                    CommitCurrent();
            }
            // Освобождаем мышь
            drawCanvas.ReleaseMouseCapture();
            // Убираем предпросмотр, если фигура не была добавлена
            RemovePreview();
            current = null;
            e.Handled = true;
        }
        // предпросмотр
        private void ShowPreview()
        {
            RemovePreview();
            if (current == null) return;
            previewShape = current.CreateWpfShape();
            if (previewShape != null)
                drawCanvas.Children.Add(previewShape);
        }
        private void RemovePreview()
        {
            if (previewShape != null)
            {
                drawCanvas.Children.Remove(previewShape);
                previewShape = null;
            }
        }
        private void CommitCurrent()
        {
            RemovePreview();
            shapes.Add(current);
            var wpfShape = current.CreateWpfShape();
            if (wpfShape != null)
                drawCanvas.Children.Add(wpfShape);
        }
        // Ctrl+Z
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (shapes.Count > 0)
                {
                    shapes.RemoveAt(shapes.Count - 1);
                    // Удаляем последний добавленный визуальный элемент
                    if (drawCanvas.Children.Count > 0)
                        drawCanvas.Children.RemoveAt(drawCanvas.Children.Count - 1);
                }
                e.Handled = true;
            }
        }
    }
    public enum ShapeType { Line, Circle, Rectangle, Freehand }
    public class Shape
    {
        public ShapeType Type;
        public WpfPoint Start;
        public WpfPoint End;
        public WpfColor Color;
        public double Thickness;
        public List<WpfPoint> Points = new List<WpfPoint>();
        public System.Windows.Shapes.Shape CreateWpfShape()
        {
            var brush = new SolidColorBrush(Color);
            switch (Type)
            {
                case ShapeType.Line:
                    return new Line
                    {
                        X1 = Start.X,
                        Y1 = Start.Y,
                        X2 = End.X,
                        Y2 = End.Y,
                        Stroke = brush,
                        StrokeThickness = Thickness
                    };
                case ShapeType.Circle:
                    var rect = MakeSquare(Start, End);
                    var ellipse = new Ellipse
                    {
                        Stroke = brush,
                        StrokeThickness = Thickness
                    };
                    Canvas.SetLeft(ellipse, rect.X);
                    Canvas.SetTop(ellipse, rect.Y);
                    ellipse.Width = rect.Width;
                    ellipse.Height = rect.Height;
                    return ellipse;
                case ShapeType.Rectangle:
                    var r = MakeRect(Start, End);
                    var rectangle = new Rectangle
                    {
                        Stroke = brush,
                        StrokeThickness = Thickness
                    };
                    Canvas.SetLeft(rectangle, r.X);
                    Canvas.SetTop(rectangle, r.Y);
                    rectangle.Width = r.Width;
                    rectangle.Height = r.Height;
                    return rectangle;
                case ShapeType.Freehand:
                    if (Points.Count == 1)
                    {
                        var dot = new Ellipse
                        {
                            Fill = brush,
                            Width = Thickness,
                            Height = Thickness
                        };
                        Canvas.SetLeft(dot, Points[0].X - Thickness / 2);
                        Canvas.SetTop(dot, Points[0].Y - Thickness / 2);
                        return dot;
                    }
                    else if (Points.Count >= 2)
                    {
                        var polyline = new Polyline
                        {
                            Stroke = brush,
                            StrokeThickness = Thickness,
                            StrokeLineJoin = PenLineJoin.Round,
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round
                        };
                        foreach (var p in Points)
                            polyline.Points.Add(p);
                        return polyline;
                    }
                    return null;
            }
            return null;
        }
        public static WpfRect MakeRect(WpfPoint a, WpfPoint b)
        {
            double x = Math.Min(a.X, b.X);
            double y = Math.Min(a.Y, b.Y);
            double w = Math.Abs(a.X - b.X);
            double h = Math.Abs(a.Y - b.Y);
            return new WpfRect(x, y, w, h);
        }
        public static WpfRect MakeSquare(WpfPoint a, WpfPoint b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double side = Math.Max(Math.Abs(dx), Math.Abs(dy));
            double x = dx >= 0 ? a.X : a.X - side;
            double y = dy >= 0 ? a.Y : a.Y - side;
            return new WpfRect(x, y, side, side);
        }
    }
}