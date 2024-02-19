using Common.Models;

namespace Common.Services
{
    /// <summary>
    /// Client for IndependentReserve APIs
    /// </summary>
    public interface IIndependentReserveClient
    {
        /// <summary>
        /// Get recent trades
        /// </summary>
        Task<RecentTradesResponse> GetRecentTradesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Subscribe and get new trades (see <see href="https://github.com/independentreserve/websockets/blob/master/orderbook-ticker.md#ticker-channel"/>)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<TradeEvent> SubscribeAndGetTradesAsync(CancellationToken cancellationToken);
    }
}