using System.Threading.Tasks;
using Xunit;

namespace Develappers.RedmineHourglassApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
           var config = Helpers.GetTestConfiguration();
           var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
           var bookings =  await client.TimeBookingsService.GetBookingsAsync();
        }
    }
}
