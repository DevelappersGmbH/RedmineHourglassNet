using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;

namespace Develappers.RedmineHourglassApi;

public interface ITimeLogService
{
    /// <summary>
    /// Lists all visible time logs
    /// </summary>
    /// <param name="query">The filter options.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The paged list of results.</returns>
    Task<PaginatedResult<TimeLog>> GetListAsync(TimeLogListQuery query, CancellationToken token = default);

    /// <summary>
    /// Retrieves a time log by its id.
    /// </summary>
    /// <param name="id">The id of the time log.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The time log.</returns>
    Task<TimeLog> GetAsync(int id, CancellationToken token = default);

    /// <summary>
    /// Deletes a time log.
    /// </summary>
    /// <param name="id">The id of the log.</param>
    /// <param name="token">The cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken token = default);

    /// <summary>
    /// Joins multiple time logs.
    /// </summary>
    /// <param name="ids">The ids of the time logs to join.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The time log data.</returns>
    Task<TimeLog> JoinAsync(List<int> ids, CancellationToken token = default);

    /// <summary>
    /// Joins multiple time logs.
    /// </summary>
    /// <param name="id">The ids of the time logs to split.</param>
    /// <param name="splitAt">The time to split the log at.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The new time log data.</returns>
    Task<TimeLogSplitResult> SplitAsync(int id, DateTime splitAt, CancellationToken token = default);

    /// <summary>
    /// Books a time log.
    /// </summary>
    /// <param name="id">The id of the log.</param>
    /// <param name="value">The detail data.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The time entry.</returns>
    Task<TimeEntry> BookAsync(int id, TimeBookingUpdate value, CancellationToken token = default);

    /// <summary>
    /// Updates a time log with the given values. Omitting values will keep the old values.
    /// </summary>
    /// <param name="id">The id of the time log.</param>
    /// <param name="values">The new values.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns></returns>
    Task UpdateAsync(int id, TimeLogUpdate values, CancellationToken token = default);

    /// <summary>
    /// Deletes multiple time logs at once.
    /// </summary>
    /// <param name="ids">The list of ids to delete.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns></returns>
    Task BulkDeleteAsync(List<int> ids, CancellationToken token = default);

    /// <summary>
    /// Updates multiple time logs at once
    /// </summary>
    /// <param name="values">The items to update.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns></returns>
    Task BulkUpdateAsync(List<TimeLogBulkUpdate> values, CancellationToken token = default);

    /// <summary>
    /// Creates multiple time logs at once
    /// </summary>
    /// <param name="values">The items to create.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns></returns>
    Task BulkCreateAsync(List<TimeLogBulkCreate> values, CancellationToken token = default);

    /// <summary>
    /// Books multiple time logs at once
    /// </summary>
    /// <param name="values">The items to book</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns></returns>
    Task BulkBookAsync(List<TimeBookingBulkUpdate> values, CancellationToken token = default);
}