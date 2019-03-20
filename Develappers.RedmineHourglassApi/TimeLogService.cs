using Develappers.RedmineHourglassApi.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Develappers.RedmineHourglassApi
{
    public class TimeLogService : BaseService, ITimeLogService
    {
        /// <inheritdoc />
        internal TimeLogService(Configuration configuration) : base(configuration)
        {
        }

        /// <inheritdoc />
        public async Task<PaginatedResult<TimeLog>> GetListAsync(BaseListQuery query, CancellationToken token = default(CancellationToken))
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await GetListAsync<TimeLog>(new Uri($"time_logs.json?offset={query.Offset}&limit={query.Limit}", UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TimeLog> GetAsync(int id, CancellationToken token = default(CancellationToken))
        {
            return await GetAsync<TimeLog>(new Uri($"time_logs/{id}.json", UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id, CancellationToken token = default(CancellationToken))
        {
            await DeleteAsync(new Uri($"time_logs/{id}.json", UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TimeLog> JoinAsync(List<int> ids, CancellationToken token = default(CancellationToken))
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            if (ids.Count <= 1)
            {
                throw new ArgumentException("at least 2 logs are needed to execute join", nameof(ids));
            }

            var queryParams = string.Join("&", ids.Select(x => $"ids[]={x}"));
            return await PostAsync<TimeLog, object>(new Uri($"time_logs/join.json?{queryParams}", UriKind.Relative), null, token).ConfigureAwait(false);
        }


        /// <inheritdoc />
        public async Task<TimeLogSplitResult> SplitAsync(int id, DateTime splitAt, CancellationToken token = default(CancellationToken))
        {
            return await PostAsync<TimeLogSplitResult, object>(new Uri($"time_logs/{id}/split.json?split_at={splitAt:o}", UriKind.Relative), null, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TimeEntry> BookAsync(int id, TimeBookingUpdate value, CancellationToken token = default(CancellationToken))
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return await PostAsync<TimeEntry, TimeLogBookRequest>(new Uri($"time_logs/{id}/book.json", UriKind.Relative), new TimeLogBookRequest { Values = value }, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(int id, TimeLogUpdate values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            await UpdateAsync(new Uri($"time_logs/{id}.json", UriKind.Relative), new TimeLogUpdateRequest { Values = values }, token).ConfigureAwait(false);
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

            var queryParams = string.Join("&", ids.Select(x => $"time_logs[]={x}"));
            await BulkDeleteAsync(new Uri($"time_logs/bulk_destroy.json?{queryParams}", UriKind.Relative), token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task BulkUpdateAsync(List<TimeLogBulkUpdate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var dict = new Dictionary<string, TimeLogBulkUpdate>();
            for (var i = 0; i < values.Count; i++)
            {
                dict.Add($"additionalProp{i + 1}", values[i]);
            }
            var request = new TimeLogBulkUpdateRequest { Values = dict };
            await BulkUpdateAsync(new Uri("time_logs/bulk_update.json", UriKind.Relative), request, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task BulkCreateAsync(List<TimeLogBulkCreate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            await BulkCreateAsync(new Uri("time_logs/bulk_create.json", UriKind.Relative), new TimeLogBulkCreateRequest { Values = values }, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task BulkBookAsync(List<TimeBookingBulkUpdate> values, CancellationToken token = default(CancellationToken))
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var dict = new Dictionary<string, TimeBookingBulkUpdate>();
            for (var i = 0; i < values.Count; i++)
            {
                dict.Add($"additionalProp{i + 1}", values[i]);
            }
            var request = new TimeLogBulkBookRequest { Values = dict };

            await BulkUpdateAsync(new Uri("time_logs/bulk_book.json", UriKind.Relative), request, token).ConfigureAwait(false);
        }
    }
}
