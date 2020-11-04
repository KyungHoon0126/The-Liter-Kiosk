using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace THE_LITER_KIOSK.Converters
{
    class BackgroundColorConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e6dff4")) : Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
