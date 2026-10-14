using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using WpfColor = System.Windows.Media.Color;
using WpfColors = System.Windows.Media.Colors;
using WpfPoint = System.Windows.Point;
namespace WpfApp1
{
    public class ShapeDrawer
    {
        private readonly Canvas _canvas;
        private Shape _currentShape;              // текущая (ещё не завершённая) фигура
        private System.Windows.Shapes.Shape _previewShape; // визуальный предпросмотр
        private bool _isDrawing;
        public ShapeType CurrentTool { get; set; } = ShapeType.Line;
        public WpfColor CurrentColor { get; set; } = WpfColors.Black;
        public double CurrentThickness { get; set; } = 2;
        public List<Shape> Shapes { get; } = new List<Shape>();
        public ShapeDrawer(Canvas canvas)
        {
            _canvas = canvas;
        }
        // Мышь
        public void OnMouseDown(MouseButtonEventArgs e, WpfPoint position)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            _isDrawing = true;
            _currentShape = new Shape
            {
                Type = CurrentTool,
                Start = position,
                End = position,
                Color = CurrentColor,
                Thickness = CurrentThickness
            };
            if (CurrentTool == ShapeType.Freehand)
                _currentShape.Points.Add(position);
            // Захватываем мышь, чтобы MouseMove/MouseUp приходили даже за пределами Canvas
            _canvas.CaptureMouse();
            _canvas.Focus();
            ShowPreview();
            e.Handled = true;
        }
        public void OnMouseMove(MouseEventArgs e, WpfPoint position)
        {
            if (!_isDrawing || _currentShape == null) return;

            if (_currentShape.Type == ShapeType.Freehand)
                _currentShape.Points.Add(position);
            else
                _currentShape.End = position;

            ShowPreview();
        }
        public void OnMouseUp(MouseButtonEventArgs e, WpfPoint position)
        {
            if (!_isDrawing || _currentShape == null) return;

            _isDrawing = false;

            if (_currentShape.Type == ShapeType.Freehand)
            {
                _currentShape.Points.Add(position);
                if (_currentShape.Points.Count > 1)
                    CommitCurrent();
            }
            else
            {
                _currentShape.End = position;
                if (_currentShape.Start != _currentShape.End)
                    CommitCurrent();
            }
            _canvas.ReleaseMouseCapture();
            RemovePreview();
            _currentShape = null;
            e.Handled = true;
        }
        // Предпросмотр
        private void ShowPreview()
        {
            RemovePreview();
            if (_currentShape == null) return;

            _previewShape = _currentShape.CreateWpfShape();
            if (_previewShape != null)
                _canvas.Children.Add(_previewShape);
        }
        private void RemovePreview()
        {
            if (_previewShape != null)
            {
                _canvas.Children.Remove(_previewShape);
                _previewShape = null;
            }
        }
        // Фиксация
        private void CommitCurrent()
        {
            RemovePreview();

            Shapes.Add(_currentShape);

            var wpfShape = _currentShape.CreateWpfShape();
            if (wpfShape != null)
                _canvas.Children.Add(wpfShape);
        }
        // Очистка / Undo
        public void Clear()
        {
            _canvas.Children.Clear();
            Shapes.Clear();
            _currentShape = null;
            _previewShape = null;
            _isDrawing = false;
        }
        public void Undo()
        {
            if (Shapes.Count == 0) return;
            Shapes.RemoveAt(Shapes.Count - 1);
            if (_canvas.Children.Count > 0)
                _canvas.Children.RemoveAt(_canvas.Children.Count - 1);
        }
    }
}