using System;

namespace Develappers.RedmineHourglassApi
{
    public class TimeBookingListFilter : IListFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}