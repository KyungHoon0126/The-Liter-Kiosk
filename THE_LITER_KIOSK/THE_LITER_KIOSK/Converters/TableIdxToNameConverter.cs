using System;
using System.Globalization;
using System.Windows.Data;

namespace THE_LITER_KIOSK.Converters
{
    public class TableIdxToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int tableIdx = (int)value;

            if (tableIdx == -1)
            {
                return "포장주문";
            }
            else
            {
                return $"매장식사-{tableIdx}번 테이블";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
