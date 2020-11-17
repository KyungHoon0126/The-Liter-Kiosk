using System;
using System.Globalization;
using System.Windows.Data;

namespace THE_LITER_KIOSK.Converters
{
    public class IsSoldOutToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "O" : "X";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
