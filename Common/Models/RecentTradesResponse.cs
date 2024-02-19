namespace Common.Models
{
    /// <summary>
    /// Recent trades from IndependentReserve API 
    /// </summary>
    public class RecentTradesResponse
    {
        /// <summary>
        /// Trades
        /// </summary>
        public required IList<RecentTradeModel> Trades { get; set; }
    }

    /// <summary>
    /// Trade
    /// </summary>
    public class RecentTradeModel
    {
        /// <summary>
        /// Trade's timestamp
        /// </summary>
        public DateTime TradeTimestampUtc { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal PrimaryCurrencyAmount { get; set; }
    }
}
