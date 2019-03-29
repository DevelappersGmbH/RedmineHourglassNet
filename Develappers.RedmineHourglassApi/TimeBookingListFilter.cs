using System;

namespace Develappers.RedmineHourglassApi
{
    public class TimeBookingListFilter : IListFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? UserId { get; set; }
        public int? IssueId { get; set; }
        public int? ProjectId { get; set; }
        public int? ActivityId { get; set; }
    }
}