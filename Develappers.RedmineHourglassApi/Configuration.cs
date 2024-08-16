using System.Diagnostics.CodeAnalysis;

namespace Develappers.RedmineHourglassApi
{
    [Serializable]
    public class Configuration 
    {
        public Configuration()
        {
        }

        [SetsRequiredMembers]
        public Configuration(string redmineUrl, string apiKey)
        {
            RedmineUrl = redmineUrl;
            ApiKey = apiKey;
        }

        /// <summary>
        /// The redmine installation url.
        /// </summary>
        public required string RedmineUrl { get; set; }

        /// <summary>
        /// The api key.
        /// </summary>
        public required string ApiKey { get; set; }

        internal Configuration DeepClone()
        {
            return new Configuration(RedmineUrl, ApiKey);
        }
    }
}
