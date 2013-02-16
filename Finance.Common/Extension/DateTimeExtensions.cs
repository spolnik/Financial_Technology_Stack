using System;

namespace Finance.Common.Extension
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the current DateTime and adjusts it by the specified TimeSpan
        /// </summary>
        /// <param name="span">Time span which will be added to now date.</param>
        /// <returns>Date time which is a now plus a span.</returns>
        public static DateTime FromNow(this TimeSpan span)
        {
            return DateTime.Now + span;
        }

        public static DateTime FromUtcNow(this TimeSpan span)
        {
            return DateTime.UtcNow + span;
        }

        public static DateTime StartOfWeek(this DateTime value)
        {
            var diff = value.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
                diff += 7;

            return value.AddDays(-1 * diff).Date;

        }

        public static DateTime NextDay(this DateTime value, DayOfWeek dayOfWeek)
        {
            var offsetDays = dayOfWeek - value.DayOfWeek;

            if (offsetDays <= 0)
                offsetDays += 7;

            return value.AddDays(offsetDays);
        }

        public static DateTime Midnight(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day);
        }

        public static DateTime SetTime(this DateTime value, int hour, int minute)
        {
            return SetTime(value, hour, minute, 0, 0);
        }

        public static DateTime SetTime(this DateTime value, int hour, int minute, int second)
        {
            return SetTime(value, hour, minute, second, 0);
        }

        public static DateTime SetTime(this DateTime value, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(value.Year, value.Month, value.Day, hour, minute, second, millisecond);
        }

        public static DateTime ForceUtc(this DateTime value)
        {
            return value.Kind == DateTimeKind.Utc
                       ? value
                       : new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second,
                                      value.Millisecond, DateTimeKind.Utc);
        }
    }
}
