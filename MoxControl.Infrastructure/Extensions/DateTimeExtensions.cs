using System.Globalization;

namespace MoxControl.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        private static TimeSpan offset { get; set; } = TimeSpan.FromHours(3);

        public static DateTime ToMoscowTime(this DateTime datetime)
        {
            var newDt = datetime + offset;
            return newDt;
        }

        public static DateTime ToUTCFromMoscowTime(this DateTime datetime)
        {
            var newDt = datetime - offset;
            return DateTime.SpecifyKind(newDt, DateTimeKind.Utc);
        }

        /// <summary>
        /// dd.mm.yyyy
        /// </summary>
        public static string GetDateOnly(this DateTime datetime) => datetime.ToString("d", DateTimeFormatInfo.CurrentInfo);

        /// <summary>
        /// dd.mm.yyyy
        /// </summary>
        public static string? GetDateOnly(this DateTime? datetime) => datetime?.ToString("d", DateTimeFormatInfo.CurrentInfo);

        /// <summary>
        /// hh:mm
        /// </summary>
        public static string GetTimeOnly(this DateTime dateTime) => dateTime.ToString("HH:mm");

        /// <summary>
        /// hh:mm
        /// </summary>
        public static string? GetTimeOnly(this DateTime? dateTime) =>
            dateTime.HasValue
                ? $"{dateTime.Value.Hour}:{dateTime.Value.Minute}"
                : null;

        /// <summary>
        /// dd.mm.yyyy hh:mm
        /// </summary>
        public static string GetDateWithTime(this DateTime datetime)
        {
            return datetime.ToString("g", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// dd.mm.yyyy hh:mm
        /// </summary>
        public static string? GetDateWithTime(this DateTime? datetime)
        {
            return datetime?.ToString("g", DateTimeFormatInfo.CurrentInfo);
        }

        public static DateTime? SetUtcDateTimeKind(this DateTime? datetime)
        {
            if (!datetime.HasValue)
                return null;

            var utcDatetime = new DateTime(datetime.Value.Ticks, DateTimeKind.Utc);
            return utcDatetime;
        }
    }
}
