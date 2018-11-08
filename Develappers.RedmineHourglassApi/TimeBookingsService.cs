using System.Net.Http;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi
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

        public async Task<PaginatedResult<TimeBooking>> GetBookingsAsync(int offset = 0, int limit = 25)
        {
            var response = await _httpClient.GetStringAsync($"time_bookings.json?offset={offset}&limit={limit}");
            return JsonConvert.DeserializeObject<PaginatedResult<TimeBooking>>(response);
        }
    }
}
