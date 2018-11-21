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

        public async Task<string> DeleteAsync(Uri relativeUri, CancellationToken token = default(CancellationToken))
        {
            var baseUri = new Uri(_hourglassUrl);
            var completeUri = new Uri(baseUri, relativeUri);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(completeUri);
            httpWebRequest.Method = "DELETE";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("X-Redmine-API-Key", _apiKey);

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


        public async Task<string> GetStringAsync(Uri relativeUri, CancellationToken token = default(CancellationToken))
        {
            var baseUri = new Uri(_hourglassUrl);
            var completeUri = new Uri(baseUri, relativeUri);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(completeUri);
            httpWebRequest.Method = "GET";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("X-Redmine-API-Key", _apiKey);

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

        public async Task<string> PutStringAsync(Uri relativeUri, string value, CancellationToken token)
        {
            var baseUri = new Uri(_hourglassUrl);
            var completeUri = new Uri(baseUri, relativeUri);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(completeUri);
            httpWebRequest.Method = "PUT";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("X-Redmine-API-Key", _apiKey);

            if (!string.IsNullOrEmpty(value))
            {
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
    }
}
