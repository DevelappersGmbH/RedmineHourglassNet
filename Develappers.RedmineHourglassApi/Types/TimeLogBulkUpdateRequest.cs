using System.Collections.Generic;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types;

/// <summary>
/// Class which wraps a <see cref="TimeLogBulkUpdate"/> object to be API conform.
/// </summary>
internal class TimeLogBulkUpdateRequest
{
    [JsonProperty("time_logs")]
    public Dictionary<string, TimeLogBulkUpdate> Values { get; set; }
}