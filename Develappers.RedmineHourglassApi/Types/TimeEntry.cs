using System;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    public class TimeEntry
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("project_id")]
        public int ProjectId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("issue_id")]
        public int? IssueId { get; set; }

        [JsonProperty("hours")]
        public decimal Hours { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("activity_id")]
        public int ActivityId { get; set; }

        [JsonProperty("spent_on")]
        public DateTime SpentOn { get; set; }

        [JsonProperty("tyear")]
        public int TYear { get; set; }

        [JsonProperty("tmonth")]
        public int TMonth { get; set; }

        [JsonProperty("tweek")]
        public int TWeek { get; set; }

        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("updated_on")]
        public DateTime UpdatedOn { get; set; }
    }
}

