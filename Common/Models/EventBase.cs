namespace Common.Models
{
    /// <summary>
    /// Base Event calss
    /// </summary>
    public class EventBase
    {
        /// <summary>
        /// Event's type <see href="https://github.com/independentreserve/websockets/blob/master/orderbook-ticker.md#ticker-channel"/>
        /// </summary>
        public required string Event { get; set; }
    }
}
