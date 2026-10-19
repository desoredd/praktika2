using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace MyTC.Converters
{
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
            => v is bool b && b ? Visibility.Collapsed : Visibility.Visible;
        public object ConvertBack(object v, Type t, object p, CultureInfo c)
            => v is Visibility vis && vis == Visibility.Collapsed;
    }
    public class StringToImageConverter : IValueConverter
    {
        public object? Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is string s && !string.IsNullOrEmpty(s) && File.Exists(s))
            {
                var b = new BitmapImage();
                b.BeginInit(); b.CacheOption = BitmapCacheOption.OnLoad;
                b.UriSource = new Uri(s); b.EndInit();
                return b;
            }
            return null;
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c)
            => throw new NotImplementedException();
    }
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is string s)
            {
                try { return new SolidColorBrush((Color)ColorConverter.ConvertFromString(s)); }
                catch { }
            }
            return Brushes.Transparent;
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c)
            => throw new NotImplementedException();
    }
}