using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Db
{
    public class Trade
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Amount (volume) of the trade
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// Trade's timestamp
        /// </summary>
        [Required]
        public DateTime DateUtc { get; set; }
    }
}
