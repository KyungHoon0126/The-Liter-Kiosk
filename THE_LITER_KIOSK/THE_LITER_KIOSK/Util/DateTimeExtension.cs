using System;

namespace THE_LITER_KIOSK.Util
{
    public static class DateTimeExtension
    {
        public static string ConvertDateTimeToDayOfTheWeek(DateTime date)
        {
            return date.ToString("tt H시 mm분 ss초 dddd");
        }

        public static string ConvertDateTime(DateTime date)
        {
            return date.ToString("tt H시 mm분 ss초");
        }
    }
}
