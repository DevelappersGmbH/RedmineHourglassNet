using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    public class TimeLogBulkUpdate : TimeLogUpdate
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}