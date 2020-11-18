using System;
using System.Globalization;
using System.Windows.Data;

namespace THE_LITER_KIOSK.Converters
{
    public class IsSoldOutToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSoldOut = (bool)value;
            
            if (isSoldOut)
            {
                return 0.2;
            }
            else
            {
                return 1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
