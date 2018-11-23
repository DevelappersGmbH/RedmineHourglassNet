using System.Collections.Generic;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    /// <summary>
    /// Class which wraps a <see cref="TimeBookingBulkUpdate"/> object to be API conform.
    /// </summary>
    internal class TimeBookingBulkCreateRequest
    {
        [JsonProperty("time_bookings")]
        public List<TimeBookingBulkCreate> Values { get; set; }
    }
}