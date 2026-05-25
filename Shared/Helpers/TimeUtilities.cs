namespace BLL.Helpers
{
    public static class TimeUtilities
    {
        // Shift rules:
        // Morning shift: 08:00 -> 20:00
        // Night shift:   20:00 -> 08:00 (next day)
        public static DateTime GetCurrentShiftStartTime()
        {
            DateTime now = DateTime.Now;

            if (now >= DateTime.Today.AddHours(8) && now < DateTime.Today.AddHours(20))
            {
                // Morning shift
                return DateTime.Today.AddHours(8);
            }

            if (now >= DateTime.Today.AddHours(20) && now <= DateTime.Today.AddDays(1).AddMilliseconds(-1))
            {
                // Night shift started today at 20:00
                return DateTime.Today.AddHours(20);
            }

            // Night shift started yesterday at 20:00 (now is between 00:00 and 08:00)
            return DateTime.Today.AddDays(-1).AddHours(20);
        }

        public static DateTime GetLastShiftStartTime()
        {
            return GetCurrentShiftStartTime().AddHours(-12);
        }

        public static DurationDto GetDurationStartTime(int duration, DateTime? durationStartTime, DateTime? durationEndTime)
        {
            DateTime now = DateTime.Now;

            DateTime fromTime;
            DateTime toTime = DateTime.Today.AddHours(8); // نهاية الشيفت الحالي = 8 صباح اليوم

            switch (duration)
            {
                case 0: // CurrentShift
                    fromTime = GetCurrentShiftStartTime();
                    toTime = now;
                    break;

                case 1: // LastShift
                    fromTime = GetLastShiftStartTime();
                    toTime = GetCurrentShiftStartTime();
                    break;

                case 2: // LastDay (full yesterday shifts)
                    fromTime = DateTime.Today.AddDays(-1).AddHours(8);  // 08:00 امبارح
                    toTime = DateTime.Today.AddHours(8);               // 08:00 النهارده
                    break;

                case 3: // LastWeek (last 7 days full shifts)
                    fromTime = DateTime.Today.AddDays(-7).AddHours(8); // 08:00 قبل 7 أيام
                    toTime = DateTime.Today.AddHours(8);              // 08:00 النهارده
                    break;

                case 4: // LastMonth (last 30 days full shifts)
                    fromTime = DateTime.Today.AddDays(-30).AddHours(8); // 08:00 قبل 30 يوم
                    toTime = DateTime.Today.AddHours(8);               // 08:00 النهارده
                    break;

                case 5: // Custom (From, To)
                    if (!durationStartTime.HasValue || !durationEndTime.HasValue)
                        throw new Exception("Custom duration requires both from and to datetime.");

                    if (durationEndTime.Value < durationStartTime.Value)
                        throw new Exception("Duration Start Time must be less than Duration End Time.");

                    fromTime = durationStartTime.Value;
                    toTime = durationEndTime.Value;
                    break;

                default:
                    throw new Exception("Invalid duration type.");
            }

            return new DurationDto { fromTime = fromTime, toTime = toTime };
        }

        public class DurationDto
        {
            public DateTime fromTime { get; set; }
            public DateTime toTime { get; set; }
        }
    }
}
