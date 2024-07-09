using System.Collections.Generic;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types;

/// <summary>
/// Class which wraps a <see cref="TimeTrackerBulkUpdate"/> object to be API conform.
/// </summary>
internal class TimeTrackerBulkUpdateRequest
{
    [JsonProperty("time_trackers")]
    public Dictionary<string, TimeTrackerBulkUpdate> Values { get; set; }
}