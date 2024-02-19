namespace Web.Models
{
    /// <summary>
    /// Trades info view model
    /// </summary>
    public class TradesViewModel
    {
        /// <summary>
        /// Average number of trades per minute
        /// </summary>
        public required double AvgNumberTradesPerMinute { get; set; }

        /// <summary>
        /// Average volume of trades per minute
        /// </summary>
        public required decimal AvgTradeVolumePerMinute { get; set; }
    }
}
