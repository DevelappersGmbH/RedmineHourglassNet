using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types;

public class TimeTrackerStartOptions
{
    [JsonProperty("issue_id")]
    public int IssueId { get; set; }

    [JsonProperty("comments")]
    public string Comments { get; set; }
}