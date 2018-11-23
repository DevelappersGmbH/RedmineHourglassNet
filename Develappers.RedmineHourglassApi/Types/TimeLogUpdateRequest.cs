using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    /// <summary>
    /// Class which wraps a <see cref="TimeLogUpdate"/> object to be API conform.
    /// </summary>
    internal class TimeLogUpdateRequest
    {
        [JsonProperty("time_log")]
        public TimeLogUpdate Values { get; set; }
    }
}