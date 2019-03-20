namespace Develappers.RedmineHourglassApi
{
    public class BaseListQuery
    {
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 25;
    }
}