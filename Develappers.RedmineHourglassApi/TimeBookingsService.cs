using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;
using Newtonsoft.Json;
using System.Threading;

namespace Develappers.RedmineHourglassApi
{
    public class TimeBookingService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates an instance of the service.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        internal TimeBookingService(Configuration configuration)
        {
            // internal constructor -> configuration is always set and valid
            _httpClient = new HttpClient(configuration.RedmineUrl, configuration.ApiKey);
        }

        /// <summary>
        /// Lists all visible time bookings
        /// </summary>
        /// <param name="filter">The filter options.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The paged list of results.</returns>
        public async Task<PaginatedResult<TimeBooking>> GetListAsync(BaseListFilter filter, CancellationToken token = default(CancellationToken))
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            
            var response = await _httpClient.GetStringAsync(new Uri($"time_bookings.json?offset={filter.Offset}&limit={filter.Limit}", UriKind.Relative), token);
            return JsonConvert.DeserializeObject<PaginatedResult<TimeBooking>>(response);
        }

        public async Task<TimeBooking> GetByIdAsync(int id, CancellationToken token = default(CancellationToken))
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

        
        internal async Task UpdateByIdAsync(int id, TimeBookingUpdate values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            try
            {
                //TODO: doesn't work by now; it always results in bad request
                var data = JsonConvert.SerializeObject(values, new JsonSerializerSettings{NullValueHandling = NullValueHandling.Ignore});
                var response = await _httpClient.PutStringAsync(new Uri($"time_bookings/{id}.json", UriKind.Relative), data, token);

            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                throw;
            }
            catch
            {
                throw;
            }
        } 

        /// <summary>
        /// Deletes a time booking.
        /// </summary>
        /// <param name="id">The id of the booking.</param>
        /// <param name="token">The cancellation token.</param>
        public async Task DeleteByIdAsync(int id, CancellationToken token = default(CancellationToken))
        {
            try
            {
                await _httpClient.DeleteAsync(new Uri($"time_bookings/{id}.json", UriKind.Relative), token);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // if it's not found, it is already deleted
                return;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes multiple time bookings at once.
        /// </summary>
        /// <param name="ids">The list of ids to delete.</param>
        /// <param name="token">The cancellation token.</param>
        public async Task DeleteMultipleAsync(List<int> ids, CancellationToken token = default(CancellationToken))
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            if (ids.Count == 0)
            {
                // no item to delete
                return;
            }

            try
            {
                var queryParams = string.Join("&", ids.Select(x => $"time_bookings[]={x}"));
                await _httpClient.DeleteAsync(new Uri($"time_bookings/bulk_destroy.json?{queryParams}", UriKind.Relative), token);
            }
            catch
            {
                throw;
            }
        }
    }
}