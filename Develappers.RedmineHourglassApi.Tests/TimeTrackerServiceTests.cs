using System.Collections.Generic;
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
           var client = new HourglassClient(config);
            config.RedmineUrl = "a";
           var logs =  await client.TimeTrackers.GetListAsync(new BaseListFilter());
        }

        [Fact]
        public async Task GetTrackingById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            var log = await client.TimeTrackers.GetAsync(105);
        }


        [Fact]
        public async Task Start()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            var log = await client.TimeTrackers.StartAsync(new TimeTrackerStartOptions
            {
                IssueId = 64,
                Comments = "test 1"
            });
        }

        [Fact]
        public async Task Stop()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            var log = await client.TimeTrackers.StopAsync(6);
        }

        [Fact]
        public async Task Delete()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeTrackers.DeleteAsync(13);
        }

        [Fact]
        public async Task Update()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeTrackers.UpdateAsync(14, new TimeTrackerUpdate
            {
                Comments = "bla1"
            });
        }

        [Fact]
        public async Task BulkDelete()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeTrackers.BulkDeleteAsync(new List<int> { 3, 4 });
        }
    }
}
