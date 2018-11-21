using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Develappers.RedmineHourglassApi.Tests
{
    public class TimeBookingServiceTests
    {
        [Fact]
        public async Task GetBookings()
        {
           var config = Helpers.GetTestConfiguration();
           var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
           var bookings =  await client.TimeBookingService.GetListAsync(new BaseListFilter());
        }

        [Fact]
        public async Task GetBookingById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            var booking = await client.TimeBookingService.GetByIdAsync(2);
        }

        [Fact]
        public async Task DeleteBookingById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            await client.TimeBookingService.DeleteByIdAsync(2);
        }

        [Fact]
        public async Task BulkDeleteBookings()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            await client.TimeBookingService.DeleteMultipleAsync(new List<int>{3,4});
        }

    }
}
