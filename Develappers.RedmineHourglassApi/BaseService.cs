using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi;

public abstract class BaseService
{
    /// <summary>
    /// Creates an instance of the service.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="logger">The logger.</param>
    protected BaseService(Configuration configuration, ILogger logger)
    {
        Logger = logger;
        // internal constructor -> configuration is always set and valid
        HttpClient = new HttpClient(configuration.RedmineUrl, configuration.ApiKey, logger);
    }

    protected ILogger Logger;

    internal HttpClient HttpClient { get; }

    /// <summary>
    /// Retrieves a paged list of items.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <param name="uri">The uri to query.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The task containing the result.</returns>
    public async Task<PaginatedResult<T>> GetListAsync<T>(Uri uri, CancellationToken token = default)
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
            Logger.LogDebug("Web exception [Unauthorized] occurred.", wex);
            throw new AuthenticationException("Missing or invalid authentication information.", wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogDebug("Web exception [Forbidden] occurred.", wex);
            throw new AuthorizationException("Not authorized to access this resource.", wex);
        }
        catch (Exception ex)
        {
            Logger.LogError($"unexpected exception {ex} occurred", ex);
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
    public async Task<T> GetAsync<T>(Uri uri, CancellationToken token = default)
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
            Logger.LogDebug("Web exception [NotFound] occurred.", wex);
            throw new NotFoundException("Specified item not found.", wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
        {
            Logger.LogDebug("Web exception [Unauthorized] occurred.", wex);
            throw new AuthenticationException("Missing or invalid authentication information.", wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogDebug("Web exception [Forbidden] occurred.", wex);
            throw new AuthorizationException("Not authorized to access this resource.", wex);
        }
        catch (Exception ex)
        {
            Logger.LogError($"unexpected exception {ex} occurred", ex);
            throw;
        }
    }

    /// <summary>
    /// Deletes a specific item.
    /// </summary>
    /// <param name="uri">The resource uri.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The task.</returns>
    public async Task DeleteAsync(Uri uri, CancellationToken token = default)
    {
        try
        {
            await HttpClient.DeleteAsync(uri, token).ConfigureAwait(false); 
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
        {
            Logger.LogDebug("Web exception [NotFound] occurred.", wex);
            throw new NotFoundException("Specified item not found.", wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
        {
            Logger.LogDebug("Web exception [Unauthorized] occurred.", wex);
            throw new AuthenticationException("Missing or invalid authentication information.", wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogDebug("Web exception [Forbidden] occurred.", wex);
            throw new AuthorizationException("Not authorized to access this resource.", wex);
        }
        catch (Exception ex)
        {
            Logger.LogError($"unexpected exception {ex} occurred", ex);
            throw;
        }
    }

    /// <summary>
    /// Deletes multiple of items.
    /// </summary>
    /// <param name="uri">The resource uri.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The task.</returns>
    public async Task BulkDeleteAsync(Uri uri, CancellationToken token = default)
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
            Logger.LogDebug("Web exception [Unauthorized] occurred.", wex);
            throw new AuthenticationException("Missing or invalid authentication information.", wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogDebug("Web exception [Forbidden] occurred.", wex);
            throw new AuthorizationException("Not authorized to access this resource.", wex);
        }
        catch (Exception ex)
        {
            Logger.LogError($"unexpected exception {ex} occurred", ex);
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
    public async Task UpdateAsync<T>(Uri uri, T values, CancellationToken token = default)
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
            Logger.LogDebug("Web exception [NotFound] occurred.", wex);
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
                Logger.LogInformation($"Exception {wex} occurred - will be rethrown as ArgumentException", wex);
                throw new ArgumentException(string.Join("\r\n", error.Message), nameof(values), wex);
            }

            throw new ArgumentException("Invalid arguments. See inner exception for details.", nameof(values), wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
        {
            Logger.LogDebug("Web exception [Unauthorized] occurred.", wex);
            throw new AuthenticationException("Missing or invalid authentication information.", wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogDebug("Web exception [Forbidden] occurred.", wex);
            throw new AuthorizationException("Not authorized to access this resource.", wex);
        }
        catch (Exception ex)
        {
            Logger.LogError($"unexpected exception {ex} occurred", ex);
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
    public async Task BulkUpdateAsync<T>(Uri uri, T values, CancellationToken token = default)
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
                Logger.LogInformation($"Exception {wex} occurred - will be rethrown as ArgumentException", wex);
                throw new ArgumentException(string.Join("\r\n", error.Message), nameof(values), wex);
            }

            throw new ArgumentException("Invalid arguments. See inner exception for details.", nameof(values), wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
        {
            Logger.LogDebug("Web exception [Unauthorized] occurred.", wex);
            throw new AuthenticationException("Missing or invalid authentication information.", wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogDebug("Web exception [Forbidden] occurred.", wex);
            throw new AuthorizationException("Not authorized to access this resource.", wex);
        }
        catch (Exception ex)
        {
            Logger.LogError($"unexpected exception {ex} occurred", ex);
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
    public async Task BulkCreateAsync<T>(Uri uri, T values, CancellationToken token = default)
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
                Logger.LogInformation($"Exception {wex} occurred - will be rethrown as ArgumentException", wex);
                throw new ArgumentException(string.Join("\r\n", error.Message), nameof(values), wex);
            }

            throw new ArgumentException("Invalid arguments. See inner exception for details.", nameof(values), wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
        {
            Logger.LogDebug("Web exception [Unauthorized] occurred.", wex);
            throw new AuthenticationException("Missing or invalid authentication information.", wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogDebug("Web exception [Forbidden] occurred.", wex);
            throw new AuthorizationException("Not authorized to access this resource.", wex);
        }
        catch (Exception ex)
        {
            Logger.LogError($"unexpected exception {ex} occurred", ex);
            throw;
        }
    }

    /// <summary>
    /// Executes a post.
    /// </summary>
    /// <typeparam name="TResult">The type of the result object.</typeparam>
    /// <typeparam name="TRequest">The type of the request object.</typeparam>
    /// <param name="uri">The uri to post to.</param>
    /// <param name="value">The value to post.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The task.</returns>
    public async Task<TResult> PostAsync<TResult, TRequest>(Uri uri, TRequest value, CancellationToken token = default)
    {
        try
        {
            string data = null;
            if (value != null)
            {
                data = JsonConvert.SerializeObject(value, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            var response = await HttpClient.PostStringAsync(uri, data, token);
            return JsonConvert.DeserializeObject<TResult>(response);
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
                Logger.LogInformation($"Exception {wex} occurred - will be rethrown as ArgumentException", wex);
                throw new ArgumentException(string.Join("\r\n", error.Message), nameof(value), wex);
            }

            throw new ArgumentException("Invalid arguments. See inner exception for details.", nameof(value), wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
        {
            Logger.LogDebug("Web exception [Unauthorized] occurred.", wex);
            throw new AuthenticationException("Missing or invalid authentication information.", wex);
        }
        catch (WebException wex)
            when (wex.Status == WebExceptionStatus.ProtocolError &&
                  (wex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogDebug("Web exception [Forbidden] occurred.", wex);
            throw new AuthorizationException("Not authorized to access this resource.", wex);
        }
        catch (Exception ex)
        {
            Logger.LogError($"unexpected exception {ex} occurred", ex);
            throw;
        }
    }
}