﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Develappers.RedmineHourglassApi
{
    /// <summary>
    /// Central object to access the API.
    /// </summary>
    public class HourglassClient : IHourglassClient
    {
        public HourglassClient(Configuration configuration, ILogger? logger = null)
        {
            logger ??= NullLogger.Instance;

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

            // clone the configuration to ensure later changes don't take any effect
            var config = configuration.DeepClone();
            TimeBookings = new TimeBookingService(config, logger);
            TimeLogs = new TimeLogService(config, logger);
            TimeTrackers = new TimeTrackerService(config, logger);
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
        public ITimeBookingService TimeBookings { get; }

        /// <summary>
        /// The service to access time logs.
        /// </summary>
        public ITimeLogService TimeLogs { get; }

        /// <summary>
        /// The service to access time logs.
        /// </summary>
        public ITimeTrackerService TimeTrackers { get; }
    }
}
