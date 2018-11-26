using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;
using Newtonsoft.Json;
using System.Threading;
using Develappers.RedmineHourglassApi.Logging;

namespace Develappers.RedmineHourglassApi
{
    public class TimeBookingService : ITimeBookingService
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

        /// <inheritdoc />
        public async Task<PaginatedResult<TimeBooking>> GetListAsync(BaseListFilter filter, CancellationToken token = default(CancellationToken))
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            try
            {
                var response = await _httpClient.GetStringAsync(new Uri($"time_bookings.json?offset={filter.Offset}&limit={filter.Limit}", UriKind.Relative), token);
                return JsonConvert.DeserializeObject<PaginatedResult<TimeBooking>>(response);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<TimeBooking> GetAsync(int id, CancellationToken token = default(CancellationToken))
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
                throw new NotFoundException($"time booking with id {id} not found", wex);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateAsync(int id, TimeBookingUpdate values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            try
            {
                var request = new TimeBookingUpdateRequest { Values = values };
                var data = JsonConvert.SerializeObject(request, new JsonSerializerSettings{NullValueHandling = NullValueHandling.Ignore});
                await _httpClient.PutStringAsync(new Uri($"time_bookings/{id}.json", UriKind.Relative), data, token);

            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException($"time booking with id {id} not found", wex);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id, CancellationToken token = default(CancellationToken))
        {
            try
            {
                await _httpClient.DeleteAsync(new Uri($"time_bookings/{id}.json", UriKind.Relative), token);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException($"time booking with id {id} not found", wex);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task BulkDeleteAsync(List<int> ids, CancellationToken token = default(CancellationToken))
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
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task BulkUpdateAsync(List<TimeBookingBulkUpdate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            try
            {
                var dict = new Dictionary<string, TimeBookingBulkUpdate>();
                for (var i = 0; i < values.Count; i++)
                {
                    dict.Add($"additionalProp{i + 1}", values[i]);
                }
                var request = new TimeBookingBulkUpdateRequest { Values = dict };
                var data = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await _httpClient.PostStringAsync(new Uri("time_bookings/bulk_update.json", UriKind.Relative), data, token);

            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.BadRequest)
            {
                // extract the error from web response
                var error = wex.ExtractError();
                if (error != null)
                {
                    // successfully deserialized an error object
                    LogProvider.GetCurrentClassLogger().InfoException($"exception {wex} occurred - will be rethrown as ArgumentException", wex);
                    throw new ArgumentException(string.Join("\r\n", error.Message), nameof(values), wex);
                }

                throw new ArgumentException("Invalid arguments. See inner exception for details.", nameof(values), wex);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task BulkCreateAsync(List<TimeBookingBulkCreate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            try
            {
                var request = new TimeBookingBulkCreateRequest { Values = values };
                var data = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await _httpClient.PostStringAsync(new Uri("time_bookings/bulk_create.json", UriKind.Relative), data, token);

            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.BadRequest)
            {

                // extract the error from web response
                var error = wex.ExtractError();
                if (error != null)
                {
                    // successfully deserialized an error object
                    LogProvider.GetCurrentClassLogger().InfoException($"exception {wex} occurred - will be rethrown as ArgumentException", wex);
                    throw new ArgumentException(string.Join("\r\n", error.Message), nameof(values), wex);
                }

                throw new ArgumentException("Invalid arguments. See inner exception for details.", nameof(values), wex);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }
    }
}