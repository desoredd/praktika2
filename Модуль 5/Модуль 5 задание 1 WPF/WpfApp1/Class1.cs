using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfColor = System.Windows.Media.Color;
using WpfPoint = System.Windows.Point;
using WpfRect = System.Windows.Rect;
namespace WpfApp1
{
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