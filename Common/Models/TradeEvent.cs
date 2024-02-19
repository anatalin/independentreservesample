namespace Common.Models
{
    /// <summary>
    /// Trade event
    /// </summary>
    public class TradeEvent: EventBase
    {
        /// <summary>
        /// Event's data
        /// </summary>
        public TradeEventData Data { get; set; }
    }

    /// <summary>
    /// Trade event data
    /// </summary>
    public class TradeEventData
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Volume (amount)
        /// </summary>
        public decimal Volume { get; set; }
    }
}
