using System.Collections.Generic;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;
using Xunit;

namespace Develappers.RedmineHourglassApi.Tests
{
    public class TimeBookingServiceTests
    {
        [Fact]
        public async Task GetBookings()
        {
           var config = Helpers.GetTestConfiguration();
           var client = new HourglassClient(config);
           var bookings =  await client.TimeBookings.GetListAsync(new BaseListFilter());
        }

        [Fact]
        public async Task GetBookingById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            var booking = await client.TimeBookings.GetAsync(2);
        }

        [Fact]
        public async Task DeleteBookingById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeBookings.DeleteAsync(2);
        }

        [Fact]
        public async Task BulkDeleteBookings()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeBookings.BulkDeleteAsync(new List<int>{3,4});
        }

        [Fact]
        public async Task UpdateBookingById()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            var booking = await client.TimeBookings.GetAsync(5);
            await client.TimeBookings.UpdateAsync(5, new TimeBookingUpdate
            {
                //Start = booking.Start,
                //Stop = booking.Stop,
                //ActivityId = booking.TimeEntry.ActivityId,
                //IssueId = booking.TimeEntry.IssueId,
                //ProjectId = booking.TimeEntry.ProjectId,
                //UserId = booking.TimeEntry.UserId,
                Comments = "bla1"
            });
        }
    }
}
