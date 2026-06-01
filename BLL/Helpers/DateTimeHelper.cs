using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo EgyptTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "Egypt Standard Time"
                    : "Africa/Cairo"
            );

        public static DateTime GetEgyptNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, EgyptTimeZone);
        }
        public static DateTime GetEgyptToday()
        {
            return GetEgyptNow().Date;
        }
        public static DateTime ToEgyptTime(DateTime utcDate)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, EgyptTimeZone);
        }
        public static DateTime ToUtc(DateTime egyptDate)
        {
            return TimeZoneInfo.ConvertTimeToUtc(egyptDate, EgyptTimeZone);
        }
    }
}
