using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;
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
            var log = await client.TimeTrackerService.GetAsync(105);
        }


        [Fact]
        public async Task Start()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            var log = await client.TimeTrackerService.StartAsync(new TimeTrackerStartOptions
            {
                IssueId = 64,
                Comments = "test 1"
            });
        }

        [Fact]
        public async Task Stop()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            var log = await client.TimeTrackerService.StopAsync(6);
        }

        [Fact]
        public async Task Delete()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            await client.TimeTrackerService.DeleteAsync(13);
        }

        [Fact]
        public async Task Update()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            await client.TimeTrackerService.UpdateAsync(14, new TimeTrackerUpdate
            {
                Comments = "bla1"
            });
        }
    }
}
