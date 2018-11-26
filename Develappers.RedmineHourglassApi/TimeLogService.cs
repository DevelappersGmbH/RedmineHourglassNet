using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Logging;
using Develappers.RedmineHourglassApi.Types;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi
{
    public class TimeLogService : ITimeLogService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates an instance of the service.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        internal TimeLogService(Configuration configuration)
        {
            // internal constructor -> configuration is always set and valid
            _httpClient = new HttpClient(configuration.RedmineUrl, configuration.ApiKey);
        }

        /// <inheritdoc />
        public async Task<PaginatedResult<TimeLog>> GetListAsync(BaseListFilter filter, CancellationToken token = default(CancellationToken))
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            try
            {
                var response = await _httpClient.GetStringAsync(new Uri($"time_logs.json?offset={filter.Offset}&limit={filter.Limit}", UriKind.Relative), token);
                return JsonConvert.DeserializeObject<PaginatedResult<TimeLog>>(response);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<TimeLog> GetAsync(int id, CancellationToken token = default(CancellationToken))
        {
            try
            {

                var response = await _httpClient.GetStringAsync(new Uri($"time_logs/{id}.json", UriKind.Relative), token);
                return JsonConvert.DeserializeObject<TimeLog>(response);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException($"time log with id {id} not found", wex);
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
                await _httpClient.DeleteAsync(new Uri($"time_logs/{id}.json", UriKind.Relative), token);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException($"time log with id {id} not found", wex);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<TimeLog> JoinAsync(List<int> ids, CancellationToken token = default(CancellationToken))
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            if (ids.Count <= 1)
            {
                throw new ArgumentException("at least 2 logs are needed to execute join", nameof(ids));
            }

            try
            {
                var queryParams = string.Join("&", ids.Select(x => $"ids[]={x}"));
                var response = await _httpClient.PostStringAsync(new Uri($"time_logs/join.json?{queryParams}", UriKind.Relative), null, token);
                return JsonConvert.DeserializeObject<TimeLog>(response);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }


        /// <inheritdoc />
        public async Task<TimeLogSplitResult> SplitAsync(int id, DateTime splitAt, CancellationToken token = default(CancellationToken))
        {
            try
            {
                var response = await _httpClient.PostStringAsync(new Uri($"time_logs/{id}/split.json?split_at={splitAt:o}", UriKind.Relative), null, token);
                return JsonConvert.DeserializeObject<TimeLogSplitResult>(response);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<TimeEntry> BookAsync(int id, TimeBookingUpdate value, CancellationToken token = default(CancellationToken))
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            try
            {
                var request = new TimeLogBookRequest { Values = value };
                var data = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var response = await _httpClient.PostStringAsync(new Uri($"time_logs/{id}/book.json", UriKind.Relative), data, token);
                return JsonConvert.DeserializeObject<TimeEntry>(response);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateAsync(int id, TimeLogUpdate values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            try
            {
                var request = new TimeLogUpdateRequest { Values = values };
                var data = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await _httpClient.PutStringAsync(new Uri($"time_logs/{id}.json", UriKind.Relative), data, token);

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
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException($"time log with id {id} not found", wex);
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
                var queryParams = string.Join("&", ids.Select(x => $"time_logs[]={x}"));
                await _httpClient.DeleteAsync(new Uri($"time_logs/bulk_destroy.json?{queryParams}", UriKind.Relative), token);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task BulkUpdateAsync(List<TimeLogBulkUpdate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            try
            {
                var dict = new Dictionary<string, TimeLogBulkUpdate>();
                for (var i = 0; i < values.Count; i++)
                {
                    dict.Add($"additionalProp{i + 1}", values[i]);
                }
                var request = new TimeLogBulkUpdateRequest { Values = dict };
                var data = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await _httpClient.PostStringAsync(new Uri("time_logs/bulk_update.json", UriKind.Relative), data, token);

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
        public async Task BulkCreateAsync(List<TimeLogBulkCreate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            try
            {
                var request = new TimeLogBulkCreateRequest { Values = values };
                var data = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await _httpClient.PostStringAsync(new Uri("time_logs/bulk_create.json", UriKind.Relative), data, token);

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
        public async Task BulkBookAsync(List<TimeBookingBulkUpdate> values, CancellationToken token = default(CancellationToken))
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
                var request = new TimeLogBulkBookRequest { Values = dict };
                var data = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await _httpClient.PostStringAsync(new Uri("time_logs/bulk_book.json", UriKind.Relative), data, token);

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
