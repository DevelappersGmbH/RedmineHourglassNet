using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types;

/// <summary>
/// Class which wraps a <see cref="TimeTrackerStartOptions"/> object to be API conform.
/// </summary>
internal class TimeTrackerStartRequest
{
    [JsonProperty("time_tracker")]
    public TimeTrackerStartOptions Values { get; set; }
}