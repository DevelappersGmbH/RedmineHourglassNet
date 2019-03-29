using System;

namespace Develappers.RedmineHourglassApi
{
    public class DateRangeFilter
    {
        public DateRangeFilter()
        {
            From = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            To = DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}