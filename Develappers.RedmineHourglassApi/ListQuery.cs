namespace Develappers.RedmineHourglassApi
{
    public abstract class ListQuery<T> : BaseListQuery where T : IListFilter, new()
    {
        protected ListQuery()
        {
            Filter = new T();
        }

        public T Filter { get;  }
    }
}