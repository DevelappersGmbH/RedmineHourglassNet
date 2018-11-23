using System;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    public class TimeBookingBulkCreate
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("stop")]
        public DateTime Stop { get; set; }

        [JsonProperty("project_id")]
        public int ProjectId { get; set; }

        [JsonProperty("activity_id")]
        public int ActivityId { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}