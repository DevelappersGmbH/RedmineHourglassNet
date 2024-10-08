﻿using Develappers.RedmineHourglassApi.Types;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Develappers.RedmineHourglassApi;

public class TimeBookingService : BaseService, ITimeBookingService
{
    /// <inheritdoc />
    internal TimeBookingService(Configuration configuration, ILogger logger) : base(configuration, logger)
    {
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<TimeBooking>> GetListAsync(TimeBookingListQuery query, CancellationToken token = default)
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var urlBuilder = new StringBuilder();
        urlBuilder.Append($"time_bookings.json?offset={query.Offset}&limit={query.Limit}");
        var filterQuery = query.Filter.ToQueryString();
        if (!string.IsNullOrEmpty(filterQuery))
        {
            urlBuilder.Append($"&{filterQuery}");
        }

        return await GetListAsync<TimeBooking>(new Uri(urlBuilder.ToString(), UriKind.Relative), token).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<TimeBooking> GetAsync(int id, CancellationToken token = default)
    {
        return await GetAsync<TimeBooking>(new Uri($"time_bookings/{id}.json", UriKind.Relative), token).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, TimeBookingUpdate values, CancellationToken token = default)
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        await UpdateAsync(new Uri($"time_bookings/{id}.json", UriKind.Relative), new TimeBookingUpdateRequest { Values = values }, token).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken token = default)
    {
        await DeleteAsync(new Uri($"time_bookings/{id}.json", UriKind.Relative), token).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task BulkDeleteAsync(List<int> ids, CancellationToken token = default)
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
    public async Task BulkUpdateAsync(List<TimeBookingBulkUpdate> values, CancellationToken token = default)
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
    public async Task BulkCreateAsync(List<TimeBookingBulkCreate> values, CancellationToken token = default)
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        await BulkCreateAsync(new Uri("time_bookings/bulk_create.json", UriKind.Relative), new TimeBookingBulkCreateRequest { Values = values }, token).ConfigureAwait(false);
    }
}