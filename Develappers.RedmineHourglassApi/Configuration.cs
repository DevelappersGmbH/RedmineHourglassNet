using System;

namespace Develappers.RedmineHourglassApi
{
    [Serializable]
    public class Configuration 
    {
        public Configuration()
        {
        }

        public Configuration(string redmineUrl, string apiKey)
        {
            RedmineUrl = redmineUrl;
            ApiKey = apiKey;
        }

        /// <summary>
        /// The redmine installation url.
        /// </summary>
        public string RedmineUrl { get; set; }

        /// <summary>
        /// The api key.
        /// </summary>
        public string ApiKey { get; set; }
    }
}
