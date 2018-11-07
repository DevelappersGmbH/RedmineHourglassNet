using System;

namespace TaurusSoftware.RedmineHourglassNet
{
    public class HourglassClient
    {
        public HourglassClient(string baseUrl, string apiKey)
        {
            TimeBookingsService = new TimeBookingsService(baseUrl, apiKey);
        }

        public TimeBookingsService TimeBookingsService { get; }
    }
}
