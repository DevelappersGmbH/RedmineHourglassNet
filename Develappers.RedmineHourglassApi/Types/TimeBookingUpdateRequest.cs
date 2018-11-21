using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    internal class TimeBookingUpdateRequest
    {
        [JsonProperty("time_booking")]
        public TimeBookingUpdate Values { get; set; }
    }
}