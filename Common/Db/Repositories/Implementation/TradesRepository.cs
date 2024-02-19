using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Db.Repositories.Implementation
{
    public class TradesRepository : ITradesRepository
    {
        private readonly TradesDbContext dbContext;

        public TradesRepository(TradesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Trade trade, CancellationToken cancellationToken)
        {
            await dbContext.Trades.AddAsync(trade, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(IEnumerable<Trade> trades, CancellationToken cancellationToken)
        {
            await dbContext.Trades.AddRangeAsync(trades, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TradesAggregateInfo> GetAggregatedTradesAsync(CancellationToken cancellationToken)
        {
            var result = dbContext.Database.SqlQuery<TradesAggregateInfo>(@$"select count(*) as Count, coalesce(sum(Amount), 0) as Sum, 
                                                                        min(DateUtc) as MinTradeDateUtc, max(DateUtc) as MaxTradeDateUtc
                                                                        from Trades");
            
            return await result.FirstAsync(cancellationToken);
        }

        public async Task DeleteAllAsync(CancellationToken cancellationToken)
        {
            dbContext.Database.ExecuteSql($"delete from Trades; vacuum;");
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
