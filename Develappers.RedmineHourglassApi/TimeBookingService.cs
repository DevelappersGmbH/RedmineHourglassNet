using Develappers.RedmineHourglassApi.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Develappers.RedmineHourglassApi
{
    public class TimeBookingService : BaseService, ITimeBookingService
    {
        /// <inheritdoc />
        internal TimeBookingService(Configuration configuration) : base(configuration)
        {
        }

        /// <inheritdoc />
        public async Task<PaginatedResult<TimeBooking>> GetListAsync(TimeBookingListFilter filter, CancellationToken token = default(CancellationToken))
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var urlBuilder = new StringBuilder();
            urlBuilder.Append($"time_bookings.json?offset={filter.Offset}&limit={filter.Limit}");
            
            if (filter.From.HasValue && filter.To.HasValue)
            {
                urlBuilder.Append($"&date=><{filter.From.Value.ToString("yyyy-MM-dd")}|{filter.To.Value.ToString("yyyy-MM-dd")}");
            }

            return await GetListAsync<TimeBooking>(new Uri(urlBuilder.ToString(), UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TimeBooking> GetAsync(int id, CancellationToken token = default(CancellationToken))
        {
            return await GetAsync<TimeBooking>(new Uri($"time_bookings/{id}.json", UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(int id, TimeBookingUpdate values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            await UpdateAsync(new Uri($"time_bookings/{id}.json", UriKind.Relative), new TimeBookingUpdateRequest { Values = values }, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id, CancellationToken token = default(CancellationToken))
        {
            await DeleteAsync(new Uri($"time_bookings/{id}.json", UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task BulkDeleteAsync(List<int> ids, CancellationToken token = default(CancellationToken))
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            if (ids.Count == 0)
            {
                // no item to delete
                return;
            }

            var queryParams = string.Join("&", ids.Select(x => $"time_bookings[]={x}"));
            await BulkDeleteAsync(new Uri($"time_bookings/bulk_destroy.json?{queryParams}", UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task BulkUpdateAsync(List<TimeBookingBulkUpdate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var dict = new Dictionary<string, TimeBookingBulkUpdate>();
            for (var i = 0; i < values.Count; i++)
            {
                dict.Add($"additionalProp{i + 1}", values[i]);
            }
            var request = new TimeBookingBulkUpdateRequest { Values = dict };

            await BulkUpdateAsync(new Uri("time_bookings/bulk_update.json", UriKind.Relative), request, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task BulkCreateAsync(List<TimeBookingBulkCreate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            await BulkCreateAsync(new Uri("time_bookings/bulk_create.json", UriKind.Relative), new TimeBookingBulkCreateRequest { Values = values }, token).ConfigureAwait(false);
        }
    }
}