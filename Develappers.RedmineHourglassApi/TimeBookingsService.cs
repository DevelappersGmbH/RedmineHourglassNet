using System;
using System.Net.Http;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;
using Newtonsoft.Json;
using System.Threading;

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

        public async Task<PaginatedResult<TimeBooking>> GetBookingsAsync(BaseListFilter filter, CancellationToken token = default(CancellationToken))
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var response = await _httpClient.GetStringAsync($"time_bookings.json?offset={filter.Offset}&limit={filter.Limit}");
            return JsonConvert.DeserializeObject<PaginatedResult<TimeBooking>>(response);
        }
    }
}