using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi.Types;

/// <summary>
/// Class which wraps a <see cref="TimeBookingUpdate"/> object to be API conform.
/// </summary>
internal class TimeLogBookRequest
{
    [JsonProperty("time_booking")]
    public TimeBookingUpdate Values { get; set; }
}