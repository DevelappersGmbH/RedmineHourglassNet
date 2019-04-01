using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Develappers.RedmineHourglassApi.Types;

namespace Develappers.RedmineHourglassApi
{
    public interface ITimeBookingService
    {
        /// <summary>
        /// Lists all visible time bookings
        /// </summary>
        /// <param name="query">The filter options.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The paged list of results.</returns>
        Task<PaginatedResult<TimeBooking>> GetListAsync(TimeBookingListQuery query, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Retrieves a time booking by it's id.
        /// </summary>
        /// <param name="id">The id of the time booking.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The time booking.</returns>
        Task<TimeBooking> GetAsync(int id, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Updates a time booking with the given values. Omitting values will keep the old values.
        /// </summary>
        /// <param name="id">The id of the time booking.</param>
        /// <param name="values">The new values.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateAsync(int id, TimeBookingUpdate values, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Deletes a time booking.
        /// </summary>
        /// <param name="id">The id of the booking.</param>
        /// <param name="token">The cancellation token.</param>
        Task DeleteAsync(int id, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Deletes multiple time bookings at once.
        /// </summary>
        /// <param name="ids">The list of ids to delete.</param>
        /// <param name="token">The cancellation token.</param>
        Task BulkDeleteAsync(List<int> ids, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Updates multiple time bookings at once
        /// </summary>
        /// <param name="values">The items to update.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns></returns>
        Task BulkUpdateAsync(List<TimeBookingBulkUpdate> values, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Creates multiple time bookings at once
        /// </summary>
        /// <param name="values">The items to create.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns></returns>
        Task BulkCreateAsync(List<TimeBookingBulkCreate> values, CancellationToken token = default(CancellationToken));
    }
}