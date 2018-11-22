using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Develappers.RedmineHourglassApi
{
    internal class HttpClient
    {
        private readonly string _hourglassUrl;
        private readonly string _apiKey;

        public HttpClient(string redmineBaseUrl, string apiKey)
        {
            _hourglassUrl = redmineBaseUrl;
            if (!_hourglassUrl.EndsWith("/"))
            {
                _hourglassUrl += "/";
            }
            _hourglassUrl += Constants.HourglassRelativeUrl;

            _apiKey = apiKey;
        }

        private async Task<string> ExecuteRequestInternalAsync(string method, Uri relativeUri, string value, CancellationToken token)
        {
            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (method != "GET" && method != "PUT" && method != "POST" && method != "DELETE")
            {
                throw new ArgumentException("invalid request method", nameof(method));
            }

            if (relativeUri == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var baseUri = new Uri(_hourglassUrl);
            var completeUri = new Uri(baseUri, relativeUri);

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

            return result;
        }

        public Task<string> GetStringAsync(Uri relativeUri, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequestInternalAsync("GET", relativeUri, null, token);
        }

        public Task<string> PutStringAsync(Uri relativeUri, string value, CancellationToken token)
        {
            return ExecuteRequestInternalAsync("PUT", relativeUri, value, token);
        }

        public Task<string> PostStringAsync(Uri relativeUri, string value, CancellationToken token)
        {
            return ExecuteRequestInternalAsync("POST", relativeUri, value, token);
        }

        public Task<string> DeleteAsync(Uri relativeUri, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequestInternalAsync("DELETE", relativeUri, null, token);
        }
    }
}
