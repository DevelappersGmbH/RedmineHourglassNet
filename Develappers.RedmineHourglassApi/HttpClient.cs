using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Develappers.RedmineHourglassApi;

internal class HttpClient
{
    private readonly string _hourglassUrl;
    private readonly string _apiKey;
    private readonly ILogger _logger;

    public HttpClient(string redmineBaseUrl, string apiKey, ILogger logger)
    {
        _hourglassUrl = redmineBaseUrl;
        if (!_hourglassUrl.EndsWith("/"))
        {
            _hourglassUrl += "/";
        }
        _hourglassUrl += Constants.HourglassRelativeUrl;

        _apiKey = apiKey;
        _logger = logger;
    }

    private async Task<string> ExecuteRequestInternalAsync(string method, Uri relativeUri, string value, CancellationToken token)
    {
        if (string.IsNullOrEmpty(method))
        {
            _logger.LogError($"{nameof(ExecuteRequestInternalAsync)} called with invalid method");
            throw new ArgumentNullException(nameof(method));
        }

        if (method != "GET" && method != "PUT" && method != "POST" && method != "DELETE")
        {
            _logger.LogError($"{nameof(ExecuteRequestInternalAsync)} called with unsupported method");
            throw new ArgumentException("invalid request method", nameof(method));
        }

        if (relativeUri == null)
        {
            throw new ArgumentNullException(nameof(relativeUri));
        }

        var baseUri = new Uri(_hourglassUrl);
        var completeUri = new Uri(baseUri, relativeUri);

        _logger.LogDebug($"executing {method} request to {completeUri} with body '{value}'" );

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(completeUri);
        httpWebRequest.Method = method;
        httpWebRequest.Accept = "application/json";
            
        httpWebRequest.Headers.Add("X-Redmine-API-Key", _apiKey);

        if (!string.IsNullOrEmpty(value))
        {
            // we always use json
            httpWebRequest.ContentType = "application/json";

            var reqStream = httpWebRequest.GetRequestStream();
            var bytes = Encoding.UTF8.GetBytes(value);
            await reqStream.WriteAsync(bytes, 0, bytes.Length, token).ConfigureAwait(false);
            reqStream.Close();
        }

        string result;
        using (var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync())
        {
            var responseStream = httpResponse.GetResponseStream();
            if (responseStream == null)
            {
                throw new IOException("response stream was null!");
            }

            using (var streamReader = new StreamReader(responseStream))
            {
                result = await streamReader.ReadToEndAsync();
            }
        }

        _logger.LogDebug($"retrieved result '{result}'");
        return result;
    }

    public Task<string> GetStringAsync(Uri relativeUri, CancellationToken token = default)
    {
        return ExecuteRequestInternalAsync("GET", relativeUri, null, token);
    }

    public Task<string> PutStringAsync(Uri relativeUri, string value, CancellationToken token = default)
    {
        return ExecuteRequestInternalAsync("PUT", relativeUri, value, token);
    }

    public Task<string> PostStringAsync(Uri relativeUri, string value, CancellationToken token = default)
    {
        return ExecuteRequestInternalAsync("POST", relativeUri, value, token);
    }

    public Task<string> DeleteAsync(Uri relativeUri, CancellationToken token = default)
    {
        return ExecuteRequestInternalAsync("DELETE", relativeUri, null, token);
    }
}