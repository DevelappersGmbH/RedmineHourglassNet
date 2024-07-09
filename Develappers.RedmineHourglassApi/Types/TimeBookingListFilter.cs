namespace Develappers.RedmineHourglassApi.Types;

public class TimeBookingListFilter : ListFilter
{
    [HourglassFilterOperation("date")]
    public DateRangeFilter Start { get; set; }
    [HourglassFilterOperation("user_id")]
    public NumberFilter UserId { get; set; }
    [HourglassFilterOperation("issue_id")]
    public NumberFilter IssueId { get; set; }
    [HourglassFilterOperation("project_id")]
    public NumberFilter ProjectId { get; set; }
    [HourglassFilterOperation("activity_id")]
    public NumberFilter ActivityId { get; set; }
}