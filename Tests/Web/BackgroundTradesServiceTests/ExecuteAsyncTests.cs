using Common.Db.Repositories;
using Common.Services;
using Web.Services;
using Moq;
using Common.Models;
using Common.Db;
using FluentAssertions;

namespace Tests.Web.BackgroundTradesServiceTests
{
    public class ExecuteAsyncTests
    {
        [Fact]
        public async Task Always_ExecutesCorrectly()
        {
            // arrange
            var tradesRepositoryMock = new Mock<ITradesRepository>(MockBehavior.Strict);
            var independentReserveClientMock = new Mock<IIndependentReserveClient>(MockBehavior.Strict);

            var mockSequence = new MockSequence();

            tradesRepositoryMock.InSequence(mockSequence)
                .Setup(s => s.DeleteAllAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            independentReserveClientMock.InSequence(mockSequence)
                .Setup(s => s.GetRecentTradesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new RecentTradesResponse
                {
                    Trades = new[]
                    {
                        new RecentTradeModel
                        {
                            PrimaryCurrencyAmount = 12.34m,
                            TradeTimestampUtc = new DateTime(2024, 01, 05),
                        },
                        new RecentTradeModel
                        {
                            PrimaryCurrencyAmount = 6.96m,
                            TradeTimestampUtc = new DateTime(2023, 10, 15),
                        },
                    }
                }));

            var addedRecentTrades = new List<Trade>();
            tradesRepositoryMock.InSequence(mockSequence)
                .Setup(s => s.AddAsync(It.IsAny<IEnumerable<Trade>>(), It.IsAny<CancellationToken>()))
                .Callback<IEnumerable<Trade>, CancellationToken>((t, _) => addedRecentTrades.AddRange(t))
                .Returns(Task.CompletedTask);


            independentReserveClientMock.InSequence(mockSequence)
                .Setup(s => s.SubscribeAndGetTradesAsync(It.IsAny<CancellationToken>()))
                .Returns(new[]
                {
                    new TradeEvent
                    {
                        Event = "Trade",
                        Data = new TradeEventData
                        {
                            TradeDate = new DateTime(2024, 02, 15),
                            Volume = 18,
                        },
                    },
                    new TradeEvent
                    {
                        Event = "Trade",
                        Data = new TradeEventData
                        {
                            TradeDate = new DateTime(2023, 11, 12),
                            Volume = 23,
                        },
                    }
                }.ToAsyncEnumerable());

            var addedTrades = new List<Trade>();
            tradesRepositoryMock
                        .Setup(s => s.AddAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()))
                        .Callback<Trade, CancellationToken>((t, _) => addedTrades.Add(t))
                        .Returns(Task.CompletedTask);

            var service = new BackgroundTradesService(tradesRepositoryMock.Object, independentReserveClientMock.Object);

            // act
            await service.StartAsync(CancellationToken.None);
            await service.ExecuteTask!;

            // assert
            Assert.True(addedRecentTrades.Should().BeEquivalentTo(new[] {
            new Trade
            {
                Amount = 12.34m,
                DateUtc = new DateTime(2024, 01, 05),
            },
                        new Trade
                        {
                            Amount = 6.96m,
                            DateUtc = new DateTime(2023, 10, 15),
                        },
            }) is not null);


            Assert.True(addedTrades.Should().BeEquivalentTo(new[]
            {
                new Trade
            {
                Amount = 18,
                DateUtc = new DateTime(2024, 02, 15),
            },
                new Trade
            {
                Amount = 23,
                DateUtc = new DateTime(2023, 11, 12),
            }
            }) is not null);
        }
    }
}
