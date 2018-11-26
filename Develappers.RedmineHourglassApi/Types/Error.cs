using System.Collections.Generic;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types
{
    internal class Error
    {
        [JsonProperty("message")]
        public List<string> Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
