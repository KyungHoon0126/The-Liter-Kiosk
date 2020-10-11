using System;
using System.Globalization;
using System.Windows.Data;

namespace THE_LITER_KIOSK.Converters
{
    public class MenuCategoryToKoreanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string category = value.ToString();
            string convertedCategory = string.Empty;

            switch (category)
            {
                case "ALL":
                    convertedCategory = "전체";
                    break;
                case "ADE":
                    convertedCategory = "에이드";
                    break;
                case "COFFEE":
                    convertedCategory = "커피";
                    break;
                case "DESERT":
                    convertedCategory = "디저트";
                    break;
                case "LATTE":
                    convertedCategory = "라떼";
                    break;
                case "LITERCCINO":
                    convertedCategory = "리터치노";
                    break;
                case "TEA":
                    convertedCategory = "차";
                    break;
                case "THELITERSPECIAL":
                    convertedCategory = "더리터스페셜";
                    break;
                case "YOGURS":
                    convertedCategory = "요거트";
                    break;
            }

            return convertedCategory;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
