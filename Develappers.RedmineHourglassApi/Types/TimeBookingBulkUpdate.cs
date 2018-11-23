using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    public class TimeBookingBulkUpdate : TimeBookingUpdate
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}