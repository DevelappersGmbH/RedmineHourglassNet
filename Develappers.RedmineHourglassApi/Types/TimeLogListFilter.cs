namespace Develappers.RedmineHourglassApi.Types;

public class TimeLogListFilter : ListFilter
{
    [HourglassFilterOperation("date")]
    public DateRangeFilter Start { get; set; }
    [HourglassFilterOperation("user_id")]
    public NumberFilter UserId { get; set; }
}