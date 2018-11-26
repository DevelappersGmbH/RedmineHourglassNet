using Develappers.RedmineHourglassApi.Logging;
using Develappers.RedmineHourglassApi.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Develappers.RedmineHourglassApi
{
    public class TimeLogService : BaseService, ITimeLogService
    {
        /// <inheritdoc />
        internal TimeLogService(Configuration configuration) : base(configuration)
        {
        }

        /// <inheritdoc />
        public async Task<PaginatedResult<TimeLog>> GetListAsync(BaseListFilter filter, CancellationToken token = default(CancellationToken))
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return await GetListAsync<TimeLog>(new Uri($"time_logs.json?offset={filter.Offset}&limit={filter.Limit}", UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TimeLog> GetAsync(int id, CancellationToken token = default(CancellationToken))
        {
            return await GetAsync<TimeLog>(new Uri($"time_logs/{id}.json", UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id, CancellationToken token = default(CancellationToken))
        {
            await DeleteAsync(new Uri($"time_logs/{id}.json", UriKind.Relative), token).ConfigureAwait(false);
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
                var response = await HttpClient.PostStringAsync(new Uri($"time_logs/join.json?{queryParams}", UriKind.Relative), null, token);
                return JsonConvert.DeserializeObject<TimeLog>(response);
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
                    LogProvider.GetCurrentClassLogger().InfoException($"Exception {wex} occurred - will be rethrown as ArgumentException", wex);
                    throw new ArgumentException(string.Join("\r\n", error.Message), nameof(ids), wex);
                }

                throw new ArgumentException("Invalid arguments. See inner exception for details.", nameof(ids), wex);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [Unauthorized] occurred.", wex);
                throw new AuthenticationException("Missing or invalid authentication information.", wex);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [Forbidden] occurred.", wex);
                throw new AuthorizationException("Not authorized to access this resource.", wex);
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
                var response = await HttpClient.PostStringAsync(new Uri($"time_logs/{id}/split.json?split_at={splitAt:o}", UriKind.Relative), null, token);
                return JsonConvert.DeserializeObject<TimeLogSplitResult>(response);
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
                    LogProvider.GetCurrentClassLogger().InfoException($"Exception {wex} occurred - will be rethrown as ArgumentException", wex);
                    throw new ArgumentException(string.Join("\r\n", error.Message), nameof(id), wex);
                }

                throw new ArgumentException("Invalid arguments. See inner exception for details.", nameof(id), wex);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [Unauthorized] occurred.", wex);
                throw new AuthenticationException("Missing or invalid authentication information.", wex);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [Forbidden] occurred.", wex);
                throw new AuthorizationException("Not authorized to access this resource.", wex);
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
                var response = await HttpClient.PostStringAsync(new Uri($"time_logs/{id}/book.json", UriKind.Relative), data, token);
                return JsonConvert.DeserializeObject<TimeEntry>(response);
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
                    LogProvider.GetCurrentClassLogger().InfoException($"Exception {wex} occurred - will be rethrown as ArgumentException", wex);
                    throw new ArgumentException(string.Join("\r\n", error.Message), nameof(value), wex);
                }

                throw new ArgumentException("Invalid arguments. See inner exception for details.", nameof(value), wex);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [Unauthorized] occurred.", wex);
                throw new AuthenticationException("Missing or invalid authentication information.", wex);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [Forbidden] occurred.", wex);
                throw new AuthorizationException("Not authorized to access this resource.", wex);
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

            await UpdateAsync(new Uri($"time_logs/{id}.json", UriKind.Relative), new TimeLogUpdateRequest { Values = values }, token).ConfigureAwait(false);
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

            var queryParams = string.Join("&", ids.Select(x => $"time_logs[]={x}"));
            await BulkDeleteAsync(new Uri($"time_logs/bulk_destroy.json?{queryParams}", UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task BulkUpdateAsync(List<TimeLogBulkUpdate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var dict = new Dictionary<string, TimeLogBulkUpdate>();
            for (var i = 0; i < values.Count; i++)
            {
                dict.Add($"additionalProp{i + 1}", values[i]);
            }
            var request = new TimeLogBulkUpdateRequest { Values = dict };
            await BulkUpdateAsync(new Uri("time_logs/bulk_update.json", UriKind.Relative), request, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task BulkCreateAsync(List<TimeLogBulkCreate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            await BulkCreateAsync(new Uri("time_logs/bulk_create.json", UriKind.Relative), new TimeLogBulkCreateRequest { Values = values }, token).ConfigureAwait(false);
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
                await HttpClient.PostStringAsync(new Uri("time_logs/bulk_book.json", UriKind.Relative), data, token);

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
                    LogProvider.GetCurrentClassLogger().InfoException($"Exception {wex} occurred - will be rethrown as ArgumentException", wex);
                    throw new ArgumentException(string.Join("\r\n", error.Message), nameof(values), wex);
                }

                throw new ArgumentException("Invalid arguments. See inner exception for details.", nameof(values), wex);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [Unauthorized] occurred.", wex);
                throw new AuthenticationException("Missing or invalid authentication information.", wex);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [Forbidden] occurred.", wex);
                throw new AuthorizationException("Not authorized to access this resource.", wex);
            }
            catch (Exception ex)
            {
                LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
                throw;
            }
        }
    }
}
