using System;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    public class TimeLogBulkCreate
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("stop")]
        public DateTime Stop { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}