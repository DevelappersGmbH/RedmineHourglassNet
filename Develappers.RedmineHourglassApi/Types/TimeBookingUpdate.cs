using System;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types;

public class TimeBookingUpdate
{
    [JsonProperty("start")]
    public DateTime? Start { get; set; }

    [JsonProperty("stop")]
    public DateTime? Stop { get; set; }

    [JsonProperty("user_id")]
    public int? UserId { get; set; }

    [JsonProperty("project_id")]
    public int? ProjectId { get; set; }

    [JsonProperty("activity_id")]
    public int? ActivityId { get; set; }

    [JsonProperty("issue_id")]
    public int? IssueId { get; set; }

    [JsonProperty("comments")]
    public string Comments { get; set; }
}