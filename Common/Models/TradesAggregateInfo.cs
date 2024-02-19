namespace Common.Models
{
    public class TradesAggregateInfo
    {
        public long Count { get; set; }

        public decimal Sum { get; set; }

        public DateTime? MinTradeDateUtc { get; set; } 

        public DateTime? MaxTradeDateUtc { get; set; }
    }
}
