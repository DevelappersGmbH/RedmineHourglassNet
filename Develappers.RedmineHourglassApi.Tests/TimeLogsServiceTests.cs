using System.Threading.Tasks;
using Xunit;

namespace Develappers.RedmineHourglassApi.Tests
{
    public class TimeLogsServiceTests
    {
        [Fact]
        public async Task GetBookings()
        {
           var config = Helpers.GetTestConfiguration();
           var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
           var logs =  await client.TimeLogsService.GetLogsAsync(new BaseListFilter());
        }

        [Fact]
        public async Task GetBookingById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            var log = await client.TimeLogsService.GetLogById(18);
        }
    }
}
