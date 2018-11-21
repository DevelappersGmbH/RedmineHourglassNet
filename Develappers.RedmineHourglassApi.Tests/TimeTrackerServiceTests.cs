using System.Threading.Tasks;
using Xunit;

namespace Develappers.RedmineHourglassApi.Tests
{
    public class TimeTrackerServiceTests
    {
        [Fact]
        public async Task GetTrackings()
        {
           var config = Helpers.GetTestConfiguration();
           var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
           var logs =  await client.TimeTrackerService.GetListAsync(new BaseListFilter());
        }

        [Fact]
        public async Task GetTrackingById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            var log = await client.TimeTrackerService.GetByIdAsync(105);
        }
    }
}
