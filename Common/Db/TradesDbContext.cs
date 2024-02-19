using Microsoft.EntityFrameworkCore;

namespace Common.Db
{
    public class TradesDbContext: DbContext
    {
        public virtual DbSet<Trade> Trades { get; set; }

        public TradesDbContext(DbContextOptions<TradesDbContext> options) : base(options) { }

        public TradesDbContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=sqlite/trades.db");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
