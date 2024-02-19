using Common.Db;
using Moq;
using Common.Db.Repositories.Implementation;
using Moq.EntityFrameworkCore;

namespace Tests.Common.TradesRepositoryTests
{
    public class AddAsyncTests
    {
        [Fact]
        public async Task Always_AddsOneToDb()
        {
            // arrange
            var dbContextMock = new Mock<TradesDbContext>();
            dbContextMock.Setup(s => s.Trades).ReturnsDbSet(new List<Trade>());

            var newTrade = new Trade
            {
                Amount = 1,
                DateUtc = new DateTime(2024, 05, 01),
            };

            var repo = new TradesRepository(dbContextMock.Object);

            // act
            await repo.AddAsync(newTrade, CancellationToken.None);

            // assert
            dbContextMock.Verify(s => s.Trades.AddAsync(newTrade, CancellationToken.None), Times.Once);
            dbContextMock.Verify(s => s.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Always_AddsMultipleToDb()
        {
            // arrange
            var dbContextMock = new Mock<TradesDbContext>();
            dbContextMock.Setup(s => s.Trades).ReturnsDbSet(new List<Trade>());

            var newTrades = new[] {
                new Trade
                    {
                        Amount = 1,
                        DateUtc = new DateTime(2024, 05, 01),
                    },
                new Trade
                    {
                        Amount = 3,
                        DateUtc = new DateTime(2022, 05, 01),
                    }
            };

            var repo = new TradesRepository(dbContextMock.Object);

            // act
            await repo.AddAsync(newTrades, CancellationToken.None);

            // assert
            dbContextMock.Verify(s => s.Trades.AddRangeAsync(newTrades, CancellationToken.None), Times.Once);
            dbContextMock.Verify(s => s.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}
