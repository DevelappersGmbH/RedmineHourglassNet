using System.Collections.Generic;
using System.Reflection;
using Develappers.RedmineHourglassApi.Types;

namespace Develappers.RedmineHourglassApi
{
    public abstract class ListFilter
    {
        public string ToQueryString()
        {
            var filters = new List<string>();
            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                var filter = property.Name.ToLowerInvariant(); 
                var attribute = property.GetCustomAttribute<HourglassFilterOperationAttribute>();
                if (attribute != null)
                {
                    filter = attribute.FieldName;
                }

                var value = property.GetValue(this);

                switch (value)
                {
                    case null:
                        // filter not set
                        continue;
                    case DateRangeFilter drf:
                        filter += $"=><{drf.From:yyyy-MM-dd}|{drf.To:yyyy-MM-dd}";
                        break;
                    case NumberFilter nf:
                        filter += $"={nf.Value}";
                        break;
                }

                filters.Add(filter);
            }

            return string.Join("&", filters);
        }
    }
}
