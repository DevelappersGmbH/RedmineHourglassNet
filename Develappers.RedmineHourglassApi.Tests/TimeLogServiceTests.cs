using System;
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
           var client = new HourglassClient(config);
           var logs =  await client.TimeLogs.GetListAsync(new BaseListQuery()); 
        }

        [Fact]
        public async Task GetLog()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            var log = await client.TimeLogs.GetAsync(18);
        }

        [Fact]
        public async Task DeleteLog()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeLogs.DeleteAsync(18);
        }

        [Fact]
        public async Task Book()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeLogs.BookAsync(18, new TimeBookingUpdate()
            {
                Comments = "blubb"
            });
        }

        [Fact]
        public async Task Join()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeLogs.JoinAsync(new List<int>{3,4});
        }

        [Fact]
        public async Task Split()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeLogs.SplitAsync(3, DateTime.Now);
        }


        [Fact]
        public async Task Update()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeLogs.UpdateAsync(14, new TimeLogUpdate
            {
                Comments = "bla1"
            });
        }


        [Fact]
        public async Task BulkDeleteLogs()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeLogs.BulkDeleteAsync(new List<int> { 3, 4 });
        }

        [Fact]
        public async Task BulkCreate()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeLogs.BulkCreateAsync(new List<TimeLogBulkCreate>
            {
                new TimeLogBulkCreate { Start = new DateTime(2018,11,23,10,0,0), Stop = new DateTime(2018,11,23,10,10,0), UserId = 13, Comments = "bla"},
                new TimeLogBulkCreate { Start = new DateTime(2018,11,23,10,11,0), Stop = new DateTime(2018,11,23,10,20,0), UserId = 13}


            });
        }
    }
}
