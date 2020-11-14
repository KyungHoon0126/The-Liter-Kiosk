using System;
using System.Globalization;
using System.Windows.Data;

namespace THE_LITER_KIOSK.Converters
{
    public class PayTimeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var payTime = (string)value;

            if (payTime == "1/1/0001 12:00:00 AM" || payTime == null || payTime == "")
            {
                return "";
            }
            else
            {
                return "결제 시간 : " + DateTime.Parse(payTime).ToString("tt H시 mm분 ss초");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
