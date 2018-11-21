using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    internal class TimeTrackerStopResponse
    {
        [JsonProperty("time_log")]
        public TimeLog TimeLog { get; set; }
    }
}