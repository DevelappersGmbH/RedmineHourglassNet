using System.Threading.Tasks;
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
        public async Task GeLogById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            var log = await client.TimeLogService.GetByIdAsync(18);
        }

        [Fact]
        public async Task DeleteLogById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config.RedmineUrl, config.ApiKey);
            await client.TimeLogService.DeleteByIdAsync(18);
        }
    }
}
