using System.Threading.Tasks;
using Xunit;

namespace Develappers.RedmineHourglassApi.Tests
{
    public class TimeBookingsServiceTests
    {
        [Fact]
        public async Task GetBookings()
        {
           var config = Helpers.GetTestConfiguration();
           var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
           var bookings =  await client.TimeBookingsService.GetBookingsAsync(new BaseListFilter());
        }

        [Fact]
        public async Task GetBookingById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            var booking = await client.TimeBookingsService.GetBookingById(1);
        }
    }
}
