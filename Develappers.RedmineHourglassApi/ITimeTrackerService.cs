using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;

namespace Develappers.RedmineHourglassApi
{
    public interface ITimeTrackerService
    {
        /// <summary>
        /// Lists all visible running time trackers.
        /// </summary>
        /// <param name="filter">The filter options.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The paged list of results.</returns>
        Task<PaginatedResult<TimeTracker>> GetListAsync(BaseListFilter filter, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Starts a new time tracker. (If there's already a started tracker, this method will throw an exception)
        /// </summary>
        /// <param name="value">The detail data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The time tracker data.</returns>
        Task<TimeTracker> StartAsync(TimeTrackerStartOptions value, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Stops a time tracker.
        /// </summary>
        /// <param name="id">The tracker id.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The time log data.</returns>
        Task<TimeLog> StopAsync(int id, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Retrieves a time tracker by it's id.
        /// </summary>
        /// <param name="id">The id of the time tracker.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The time tracker.</returns>
        Task<TimeTracker> GetAsync(int id, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Deletes a time tracker.
        /// </summary>
        /// <param name="id">The id of the tracker.</param>
        /// <param name="token">The cancellation token.</param>
        Task DeleteAsync(int id, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Updates a time tracker with the given values. Omitting values will keep the old values.
        /// </summary>
        /// <param name="id">The id of the time tracker.</param>
        /// <param name="values">The new values.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateAsync(int id, TimeTrackerUpdate values, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Deletes multiple time trackers at once.
        /// </summary>
        /// <param name="ids">The list of ids to delete.</param>
        /// <param name="token">The cancellation token.</param>
        Task BulkDeleteAsync(List<int> ids, CancellationToken token = default(CancellationToken));

        /// <inheritdoc />
        Task BulkUpdateAsync(List<TimeTrackerBulkUpdate> values, CancellationToken token = default(CancellationToken));
    }
}