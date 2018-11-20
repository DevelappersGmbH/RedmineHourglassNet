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

        public async Task<List<TimeBooking>> GetBookingsForSpecificMonthAsync(int month = 0, int year = 0)
        {
            var timeRecords = new List<TimeBooking>();
            var checkNextPage = false;
            do
            {
                var response = await _httpClient.GetStringAsync($"time_bookings.json?offset={offset}&limit={limit}");
                var timeBookings = JsonConvert.DeserializeObject<PaginatedResult<TimeBooking>>(response);
                if (timeBookings.Records.Count == 0) break;
                var shouldContinue = CheckDateValidation(month, year, timeBookings.Records.First());
                if (!shouldContinue) break;
                timeRecords.AddRange(GetBookingsForSpecificMonth(timeBookings.Records, month, year));
                UpdateOffset();

                if (timeRecords.Count == 0) checkNextPage = true;
                else if (timeRecords.Last() == timeBookings.Records.Last()) checkNextPage = true;

            } while (checkNextPage);

            ResetOffset();
            return timeRecords;
        }

        private bool CheckDateValidation(int month, int year, TimeBooking firstRecord)
        {
            var firstRecordDateTime = firstRecord.Start;
            if (year < firstRecordDateTime.Year) return false;
            else if (month < firstRecordDateTime.Month && year == firstRecordDateTime.Year) return false;
            else return true;
        }

        public List<TimeBooking> GetBookingsForSpecificMonth(List<TimeBooking> timeRecords, int month, int year=0)
        {
            //get current month and year if user did not choose specific month
            if (month == 0) {
                month = DateTime.Now.Month; 
                year = DateTime.Now.Year;
            }
            return timeRecords.Where(o => o.Start.Month == month && o.Start.Year == year).ToList();
        }

        private void ResetOffset()
        {
            offset = 0;
        }

        private void UpdateOffset()
        {
            offset += 100;
        }
    }
}