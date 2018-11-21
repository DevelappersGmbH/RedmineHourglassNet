using System;
using System.Net;
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
        /// <param name="configuration">The configuration.</param>
        internal TimeBookingsService(Configuration configuration)
        {
            // internal constructor -> configuration is always set and valid
            _httpClient = new HttpClient(configuration.RedmineUrl, configuration.ApiKey);
        }

        public async Task<PaginatedResult<TimeBooking>> GetBookingsAsync(BaseListFilter filter, CancellationToken token = default(CancellationToken))
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            
            var response = await _httpClient.GetStringAsync(new Uri($"time_bookings.json?offset={filter.Offset}&limit={filter.Limit}", UriKind.Relative), token);
            return JsonConvert.DeserializeObject<PaginatedResult<TimeBooking>>(response);
        }

        public async Task<TimeBooking> GetBookingById(int id, CancellationToken token = default(CancellationToken))
        {
            try
            {

                var response = await _httpClient.GetStringAsync(new Uri($"time_bookings/{id}.json", UriKind.Relative), token);
                return JsonConvert.DeserializeObject<TimeBooking>(response);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}