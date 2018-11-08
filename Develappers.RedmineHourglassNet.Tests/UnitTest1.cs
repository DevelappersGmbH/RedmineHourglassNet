using System.Threading.Tasks;
using Xunit;

namespace Develappers.RedmineHourglassNet.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
           var client = new HourglassClient("","");
           await client.TimeBookingsService.GetBookingsAsync();
        }
    }
}
