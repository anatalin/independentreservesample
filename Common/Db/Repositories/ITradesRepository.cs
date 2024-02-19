using Common.Models;

namespace Common.Db.Repositories
{
    public interface ITradesRepository
    {
        /// <summary>
        /// Add to DB
        /// </summary>
        Task AddAsync(IEnumerable<Trade> trades, CancellationToken cancellationToken);

        /// <summary>
        /// Add to DB
        /// </summary>
        Task AddAsync(Trade trade, CancellationToken cancellationToken);

        /// <summary>
        /// Delete all Trades from DB
        /// </summary>
        Task DeleteAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get aggregated trades info
        /// </summary>
        Task<TradesAggregateInfo> GetAggregatedTradesAsync(CancellationToken cancellationToken);
    }
}
