using System.Net.Http;
using System.Threading.Tasks;

namespace Develappers.RedmineHourglassNet
{
    public class TimeBookingsService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates an instance of the service.
        /// </summary>
        /// <param name="httpClient">The initialized http client.</param>
        internal TimeBookingsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task GetBookingsAsync()
        {
                var response = await _httpClient.GetStringAsync("time_bookings.json");
        }
    }
}
