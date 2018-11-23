using System.Collections.Generic;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;
using Xunit;

namespace Develappers.RedmineHourglassApi.Tests
{
    public class TimeLogServiceTests
    {
        [Fact]
        public async Task GetLogs()
        {
           var config = Helpers.GetTestConfiguration();
           var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
           var logs =  await client.TimeLogService.GetListAsync(new BaseListFilter());
        }

        [Fact]
        public async Task GetLog()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            var log = await client.TimeLogService.GetAsync(18);
        }

        [Fact]
        public async Task DeleteLog()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            await client.TimeLogService.DeleteAsync(18);
        }

        [Fact]
        public async Task Book()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            await client.TimeLogService.BookAsync(18, new TimeBookingUpdate()
            {
                Comments = "blubb"
            });
        }

        [Fact]
        public async Task Join()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            await client.TimeLogService.JoinAsync(new List<int>{3,4});
        }
    }
}
