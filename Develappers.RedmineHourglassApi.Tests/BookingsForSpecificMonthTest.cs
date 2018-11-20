using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Develappers.RedmineHourglassApi.Tests
{
    public class BookingsForSpecificMonthTest
    {
        [Fact]
        public async Task Test2()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            var bookingsRecords = await client.TimeBookingsService.GetBookingsForSpecificMonthAsync(10,2018);
        }
    }
}