using System;
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
           var bookings =  await client.TimeBookings.GetListAsync(new TimeBookingListFilter
           {
               From = new DateTime(2018,01,01),
               To = new DateTime(2018,12,31)
           });
           
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
        public async Task BulkCreateBookings()
        {
            var config = Helpers.GetTestConfiguration();
            var client = new HourglassClient(config);
            await client.TimeBookings.BulkCreateAsync(new List<TimeBookingBulkCreate>
            {
                new TimeBookingBulkCreate { Start = new DateTime(2018,11,23,9,0,0), Stop = new DateTime(2018,11,23,9,10,0), ProjectId = 11, ActivityId = 8, UserId = 13},
                new TimeBookingBulkCreate { Start = new DateTime(2018,11,23,9,11,0), Stop = new DateTime(2018,11,23,9,20,0), ProjectId = 11, ActivityId = 8 , UserId = 13}


            });
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
