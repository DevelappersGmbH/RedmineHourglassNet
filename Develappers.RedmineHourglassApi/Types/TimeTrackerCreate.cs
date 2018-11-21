using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    public class TimeTrackerCreate
    {
        [JsonProperty("issue_id")]
        public int IssueId { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }
    }
}