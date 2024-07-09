using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types;

public class TimeTrackerBulkUpdate: TimeTrackerUpdate
{
    [JsonProperty("id")]
    public int Id { get; set; }
}