using System;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    public class TimeBooking
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("stop")]
        public DateTime Stop { get; set; }

        [JsonProperty("time_log_id")]
        public int TimeLogId { get; set; }

        [JsonProperty("time_entry_id")]
        public int TimeEntryId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("time_entry")]
        public TimeEntry TimeEntry { get; set; }
    }
}

