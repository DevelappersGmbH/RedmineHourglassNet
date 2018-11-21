using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi
{
    public class TimeTrackerService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates an instance of the service.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        internal TimeTrackerService(Configuration configuration)
        {
            // internal constructor -> configuration is always set and valid
            _httpClient = new HttpClient(configuration.RedmineUrl, configuration.ApiKey);
        }

        /// <summary>
        /// Lists all visible running time trackers.
        /// </summary>
        /// <param name="filter">The filter options.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The paged list of results.</returns>
        public async Task<PaginatedResult<TimeTracker>> GetListAsync(BaseListFilter filter, CancellationToken token = default(CancellationToken))
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var response = await _httpClient.GetStringAsync(new Uri($"time_trackers.json?offset={filter.Offset}&limit={filter.Limit}", UriKind.Relative), token);
            return JsonConvert.DeserializeObject<PaginatedResult<TimeTracker>>(response);
        }

        public async Task<TimeTracker> StartAsync(TimeTrackerCreate value, CancellationToken token = default(CancellationToken))
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            try
            {
                var data = JsonConvert.SerializeObject(value, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var response = await _httpClient.PostStringAsync(new Uri("time_trackers/start.json", UriKind.Relative), data, token);
                return JsonConvert.DeserializeObject<TimeTracker>(response);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            catch
            {
                throw;
            }
        }

        public async Task<TimeLog> StopAsync(int id, CancellationToken token = default(CancellationToken))
        {
            try
            {
                var response = await _httpClient.DeleteAsync(new Uri($"time_trackers/{id}/stop.json", UriKind.Relative), token);
                var result = JsonConvert.DeserializeObject<TimeTrackerResult>(response);
                return result.TimeLog;
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            catch
            {
                throw;
            }
        }


        public async Task<TimeTracker> GetByIdAsync(int id, CancellationToken token = default(CancellationToken))
        {
            try
            {

                var response = await _httpClient.GetStringAsync(new Uri($"time_trackers/{id}.json", UriKind.Relative), token);
                return JsonConvert.DeserializeObject<TimeTracker>(response);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            catch
            {
                throw;
            }
        }
    }
}
