using System;

namespace Develappers.RedmineHourglassApi
{
    /// <summary>
    /// Central object to access the API.
    /// </summary>
    public class HourglassClient
    {
        public HourglassClient(Configuration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration.RedmineUrl))
            {
                throw new ArgumentException("invalid base url", nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration.ApiKey))
            {
                throw new ArgumentException("invalid api key", nameof(configuration));
            }

            // clone the configuration to ensure afterwards changes don't take any effect
            var config = configuration.DeepClone();
            TimeBookingsService = new TimeBookingsService(config);
            TimeLogsService = new TimeLogsService(config);
        }


        /// <summary>
        /// Creates a new instance of the client.
        /// </summary>
        /// <param name="baseUrl">The redmine installation url.</param>
        /// <param name="apiKey">The api key.</param>
        public HourglassClient(string baseUrl, string apiKey) : this(new Configuration(baseUrl, apiKey))
        {
        }

        /// <summary>
        /// The service to access time bookings.
        /// </summary>
        public TimeBookingsService TimeBookingsService { get; }

        /// <summary>
        /// The service to access time logs.
        /// </summary>
        public TimeLogsService TimeLogsService { get; }
    }
}
