using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Logging;
using Develappers.RedmineHourglassApi.Types;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi
{
    public abstract class BaseService
    {
        /// <summary>
        /// Creates an instance of the service.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected BaseService(Configuration configuration)
        {
            // internal constructor -> configuration is always set and valid
            HttpClient = new HttpClient(configuration.RedmineUrl, configuration.ApiKey);
        }

        internal HttpClient HttpClient { get; }

        /// <summary>
        /// Retrieves a paged list of items.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="uri">The uri to query.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task containing the result.</returns>
        public async Task<PaginatedResult<T>> GetListAsync<T>(Uri uri, CancellationToken token = default(CancellationToken))
        {
            try
            {
                var response = await HttpClient.GetStringAsync(uri, token).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<PaginatedResult<T>>(response);
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

        /// <summary>
        /// Retrieves an item from the specified url.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="uri">The uri to query.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task containing the result.</returns>
        public async Task<T> GetAsync<T>(Uri uri, CancellationToken token = default(CancellationToken))
        {
            try
            {
                var response = await HttpClient.GetStringAsync(uri, token).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [NotFound] occurred.", wex);
                throw new NotFoundException("Specified item not found.", wex);
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

        /// <summary>
        /// Deletes a specific item.
        /// </summary>
        /// <param name="uri">The resource uri.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public async Task DeleteAsync(Uri uri, CancellationToken token = default(CancellationToken))
        {
            try
            {
                await HttpClient.DeleteAsync(uri, token).ConfigureAwait(false); 
            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [NotFound] occurred.", wex);
                throw new NotFoundException("Specified item not found.", wex);
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

        /// <summary>
        /// Deletes multiple of items.
        /// </summary>
        /// <param name="uri">The resource uri.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public async Task BulkDeleteAsync(Uri uri, CancellationToken token = default(CancellationToken))
        {
            try
            {
                await HttpClient.DeleteAsync(uri, token).ConfigureAwait(false);
            }
            // Status Code 400 not available 
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

        /// <summary>
        /// Updates an item with the given values.
        /// </summary>
        /// <param name="uri">The resource uri.</param>
        /// <param name="values">The request body.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public async Task UpdateAsync<T>(Uri uri, T values, CancellationToken token = default(CancellationToken))
        {
            try
            {
                var data = JsonConvert.SerializeObject(values, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await HttpClient.PutStringAsync(uri, data, token).ConfigureAwait(false);

            }
            catch (WebException wex)
                when (wex.Status == WebExceptionStatus.ProtocolError &&
                      (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                LogProvider.GetCurrentClassLogger().DebugException("Web exception [NotFound] occurred.", wex);
                throw new NotFoundException("Specified item not found.", wex);
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

        /// <summary>
        /// Updates multiple of items.
        /// </summary>
        /// <param name="uri">The resource uri.</param>
        /// <param name="values">The request body.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public async Task BulkUpdateAsync<T>(Uri uri, T values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            try
            {
                var data = JsonConvert.SerializeObject(values, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await HttpClient.PostStringAsync(uri, data, token);
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

        /// <summary>
        /// Creates multiple items at once
        /// </summary>
        /// <param name="uri">The resource uri.</param>
        /// <param name="values">The request body.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public async Task BulkCreateAsync<T>(Uri uri, T values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            try
            {
                var data = JsonConvert.SerializeObject(values, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await HttpClient.PostStringAsync(uri, data, token);

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
