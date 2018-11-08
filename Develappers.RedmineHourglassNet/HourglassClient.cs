using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Develappers.RedmineHourglassNet
{
    /// <summary>
    /// Central object to access the API.
    /// </summary>
    public class HourglassClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates a new instance of the client.
        /// </summary>
        /// <param name="baseUrl">The redmine installation url.</param>
        /// <param name="apiKey">The api key.</param>
        public HourglassClient(string baseUrl, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("invalid base url", nameof(baseUrl));
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("invalid api key", nameof(apiKey));
            }

            var hourglassUrl = baseUrl;
            if (!hourglassUrl.EndsWith("/"))
            {
                hourglassUrl += "/";
            }
            hourglassUrl += "hourglass/";

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(hourglassUrl)
            };
            _httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            TimeBookingsService = new TimeBookingsService(_httpClient);
        }

        /// <summary>
        /// The service to access time bookings.
        /// </summary>
        public TimeBookingsService TimeBookingsService { get; }

        #region Dispose-Pattern
        private void ReleaseUnmanagedResources()
        {
            // nothing to release here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                _httpClient?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
