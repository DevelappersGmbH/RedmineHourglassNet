using System;

namespace Develappers.RedmineHourglassApi
{
    public class TimeBookingListFilter : BaseListFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}