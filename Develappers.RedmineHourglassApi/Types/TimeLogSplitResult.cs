using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    public class TimeLogSplitResult
    {
        [JsonProperty("time_log")]
        public TimeLog TimeLog1 { get; set; }

        [JsonProperty("new_time_log")]
        public TimeLog TimeLog2 { get; set; }
    }
}