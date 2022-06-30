namespace SharedData
{
    public enum ScheduleType
    {
        Daily = 0,
        Weekly = 1,
        Monthly = 2,
    }

    [Serializable]
    public class Schedule
    {
        public Schedule()
        {
            Type = ScheduleType.Daily;
            Time = TimeSpan.MinValue;
            DayOfWeek = DayOfWeek.Sunday;
            DayOfMonth = 1;
        }
        public ScheduleType Type { get; set; }
        public TimeSpan Time { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }

        public DateTime GetFirstDateTime()
        {
            return Type switch
            {
                ScheduleType.Daily => DateTime.Today + Time,
                ScheduleType.Weekly => GetNextWeekDay(DateTime.Today, DayOfWeek) + Time,
                ScheduleType.Monthly => GetNextMonthDay(DateTime.Today, DayOfMonth) + Time,
                _ => DateTime.MaxValue
            };
        }

        public DateTime GetNextDateTime(DateTime prev)
        {
            return Type switch
            {
                ScheduleType.Daily => prev.AddDays(1),
                ScheduleType.Weekly => prev.AddDays(7),
                ScheduleType.Monthly => GetNextMonthDay(prev.AddDays(1), DayOfMonth) + Time,
                _ => DateTime.MaxValue
            };
        }

        public static DateTime GetNextWeekDay(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static DateTime GetNextMonthDay(DateTime start, int day)
        {
            if (start.Day > day)
                start = start.AddMonths(1);
            int daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
            return new DateTime(start.Year, start.Month, daysInMonth > day ? day : daysInMonth);
        }
    }
}
