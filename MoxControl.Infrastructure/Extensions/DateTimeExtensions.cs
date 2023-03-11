using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
