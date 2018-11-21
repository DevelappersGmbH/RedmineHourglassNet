using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    internal class TimeTrackerResult
    {
        [JsonProperty("time_log")]
        public TimeLog TimeLog { get; set; }
    }
}