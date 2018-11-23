using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    /// <summary>
    /// Class which wraps a <see cref="TimeTrackerUpdate"/> object to be API conform.
    /// </summary>
    internal class TimeTrackerUpdateRequest
    {
        [JsonProperty("time_tracker")]
        public TimeTrackerUpdate Values { get; set; }
    }
}