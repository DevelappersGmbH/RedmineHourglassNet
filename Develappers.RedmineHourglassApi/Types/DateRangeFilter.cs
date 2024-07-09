using System;

namespace Develappers.RedmineHourglassApi.Types;

public class DateRangeFilter
{
    public DateTime From { get; set; } = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
    public DateTime To { get; set; } = DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
}