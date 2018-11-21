using System;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    public class TimeTracker
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("project_id")]
        public int? ProjectId { get; set; }

        [JsonProperty("activity_id")]
        public int? ActivityId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}