namespace Develappers.RedmineHourglassApi.Types;

public abstract class ListQuery<T> : BaseListQuery where T : ListFilter, new()
{
    public T Filter { get;  } = new();
}