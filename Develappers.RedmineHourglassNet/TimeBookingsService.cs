using System.Net.Http;
using System.Threading.Tasks;

namespace Develappers.RedmineHourglassNet
{
    public class TimeBookingsService
    {
        private readonly string _baseUrl;
        private readonly string _apiKey;

        internal TimeBookingsService(string baseUrl, string apiKey)
        {
            _baseUrl = baseUrl;
            _apiKey = apiKey;
        }

        public async Task GetBookingsAsync()
        {
            var url = $"{_baseUrl}/hourglass/time_bookings.json?key={_apiKey}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(url);
            }
        }
    }
}
