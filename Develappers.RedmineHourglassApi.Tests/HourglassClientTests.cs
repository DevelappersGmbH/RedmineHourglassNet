using System;
using Xunit;

namespace Develappers.RedmineHourglassApi.Tests
{
    public class HourglassClientTests
    {
        [Fact]
        public void ValidConfiguration_ShouldSetProperties()
        {
            var config = Helpers.GetTestConfiguration();
            var hourglassClient = new HourglassClient(config);

            Assert.NotNull(hourglassClient.TimeBookings);
            Assert.NotNull(hourglassClient.TimeLogs);
            Assert.NotNull(hourglassClient.TimeTrackers);
        }

        [Fact]
        public void NullConfiguration_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HourglassClient(null));
        }

        [Fact]
        public void InvalidApiKey_ShouldThrowArgumentException()
        {
            var config = new Configuration("https://redmineTest.de", "");

            var exception = Assert.Throws<ArgumentException>(() => new HourglassClient(config));
            Assert.Equal("invalid api key (Parameter 'configuration')", exception.Message);
        }

        [Fact]
        public void InvalidRedmineUrl_ShouldThrowArgumentException()
        {
            var config = new Configuration("", "ApiKey");

            var exception = Assert.Throws<ArgumentException>(() => new HourglassClient(config));
            Assert.Equal("invalid base url (Parameter 'configuration')", exception.Message);
        }
    }
}