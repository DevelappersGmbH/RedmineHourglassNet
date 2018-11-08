using System.Threading.Tasks;
using Xunit;

namespace Develappers.RedmineHourglassNet.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
           var config = Helpers.GetTestConfiguration();
           var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
           await client.TimeBookingsService.GetBookingsAsync();
        }
    }
}
