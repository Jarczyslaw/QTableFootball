using System;
using System.Globalization;

namespace JToolbox.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static int GetIsoWeekNumber(this DateTime date)
        {
            //https://docs.microsoft.com/pl-pl/archive/blogs/shawnste/iso-8601-week-of-year-format-in-microsoft-net
            DayOfWeek day = CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static bool IsEarlierThan(this DateTime dt1, DateTime dt2)
        {
            return DateTime.Compare(dt1.Date, dt2.Date) <= 0;
        }

        public static bool IsInRange(this DateTime dateTime, DateTime start, DateTime end)
        {
            return dateTime.IsLaterThan(start.Date) && dateTime.IsEarlierThan(end.Date);
        }

        public static bool IsLaterThan(this DateTime dt1, DateTime dt2)
        {
            return DateTime.Compare(dt1.Date, dt2.Date) >= 0;
        }

        public static bool IsNotInRange(this DateTime dateTime, DateTime start, DateTime end)
        {
            return DateTime.Compare(dateTime.Date, start.Date) < 0 || DateTime.Compare(dateTime.Date, end.Date) > 0;
        }

        public static bool IsWeekDay(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }
    }
}