using System;
using System.IO;
using System.Net;
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
    }
}
