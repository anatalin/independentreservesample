using Common.Db;
using Common.Db.Repositories;
using Common.Services;

namespace Web.Services
{
    /// <summary>
    /// Background service that gets trades and saves them to DB
    /// </summary>
    public class BackgroundTradesService : BackgroundService
    {
        private readonly ITradesRepository tradesRepository;
        private readonly IIndependentReserveClient independentReserveClient;

        public BackgroundTradesService(
            ITradesRepository tradesRepository,
            IIndependentReserveClient independentReserveClient)
        {
            this.tradesRepository = tradesRepository;
            this.independentReserveClient = independentReserveClient;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await this.tradesRepository.DeleteAllAsync(cancellationToken);
            var recentTrades = await this.independentReserveClient.GetRecentTradesAsync(cancellationToken);
            await this.tradesRepository.AddAsync(recentTrades.Trades.Select(t => new Trade
            {
                Amount = t.PrimaryCurrencyAmount,
                DateUtc = t.TradeTimestampUtc,
            }), cancellationToken);

            await foreach (var trade in this.independentReserveClient.SubscribeAndGetTradesAsync(cancellationToken))
            {
               await this.tradesRepository.AddAsync(new Trade
                {
                    Amount = trade.Data.Volume,
                    DateUtc = trade.Data.TradeDate,
                }, cancellationToken);
            }
        }
    }
}
