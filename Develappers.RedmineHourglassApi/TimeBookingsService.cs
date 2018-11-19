using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;
using Newtonsoft.Json;
using System.Linq;

namespace Develappers.RedmineHourglassApi
{
    public class TimeBookingsService
    {
        private readonly HttpClient _httpClient;
        private int offset = 0;
        private int limit = 100;

        /// <summary>
        /// Creates an instance of the service.
        /// </summary>
        /// <param name="httpClient">The initialized http client.</param>
        internal TimeBookingsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<TimeBooking>> GetBookingsAsync()
        {
            var response = await _httpClient.GetStringAsync($"time_bookings.json?offset={offset}&limit={limit}");
            return JsonConvert.DeserializeObject<PaginatedResult<TimeBooking>>(response);
        }

        public async Task<List<TimeBooking>> GetBookingsForSpecificMonthAsync(int month = 0)
        {
            var timeRecords = new List<TimeBooking>();
            var checkNextPage = false;
            do
            {
                var response = await _httpClient.GetStringAsync($"time_bookings.json?offset={offset}&limit={limit}");
                var timeBookings = JsonConvert.DeserializeObject<PaginatedResult<TimeBooking>>(response);
                if (timeBookings.Records.Count == 0) break;
                timeRecords.AddRange(GetBookingsForSpecificMonth(timeBookings.Records, month));
                UpdateOffset();
                //TODO: fix checking if no record is on the first page
                if (timeRecords.Last() == timeBookings.Records.Last()) checkNextPage = true;
            } while (checkNextPage);

            ResetOffset();
            return timeRecords;
        }

        private void ResetOffset()
        {
            offset = 0;
        }

        private void UpdateOffset()
        {
            offset += 100;
        }

        public List<TimeBooking> GetBookingsForSpecificMonth(List<TimeBooking> timeRecords, int month)
        {
            if (month == 0) month = DateTime.Now.Month; //get current month if user did not choose specific month
            return timeRecords.Where(o => o.Start.Month == month).ToList();
        }
    }
}