using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    internal class TimeTrackerCreateRequest
    {
        [JsonProperty("time_tracker")]
        public TimeTrackerCreate Options { get; set; }
    }
}