using Newtonsoft.Json;
using System;

namespace Develappers.RedmineHourglassApi.Types
{
    public class TimeLog
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("stop")]
        public DateTime Stop { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("hours")]
        public decimal Hours { get; set; }
    }
}