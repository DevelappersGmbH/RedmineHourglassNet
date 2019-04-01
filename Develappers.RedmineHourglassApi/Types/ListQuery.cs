namespace Develappers.RedmineHourglassApi.Types
{
    public abstract class ListQuery<T> : BaseListQuery where T : ListFilter, new()
    {
        protected ListQuery()
        {
            Filter = new T();
        }

        public T Filter { get;  }
    }
}