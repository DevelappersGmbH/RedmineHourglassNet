using System.Collections.Generic;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    /// <summary>
    /// Class which wraps a <see cref="TimeLogBulkCreate"/> object to be API conform.
    /// </summary>
    internal class TimeLogBulkCreateRequest
    {
        [JsonProperty("time_logs")]
        public List<TimeLogBulkCreate> Values { get; set; }
    }
}