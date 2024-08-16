namespace Develappers.RedmineHourglassApi
{
    public interface IHourglassClient
    {
        ITimeBookingService TimeBookings { get; }
        ITimeLogService TimeLogs { get; }
        ITimeTrackerService TimeTrackers { get; }
    }
}