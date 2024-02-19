using Common.Db.Repositories;
using Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITradesRepository tradesRepository;

        public TradesViewModel TradesInfoViewModel { get; set; } = new TradesViewModel
        {
            AvgNumberTradesPerMinute = 0,
            AvgTradeVolumePerMinute = 0
        };

        public IndexModel(ITradesRepository tradesRepository)
        {
            this.tradesRepository = tradesRepository;
        }

        /// <summary>
        /// Displays page with aggregated trades info
        /// </summary>
        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var info = await this.tradesRepository.GetAggregatedTradesAsync(cancellationToken);
            if (info is null || info.MinTradeDateUtc is null || info.MaxTradeDateUtc is null)
                return Page();

            var minutes = (info.MaxTradeDateUtc.Value - info.MinTradeDateUtc.Value).TotalMinutes;
            if (minutes == 0)
            {
                minutes = 1;
            }

            this.TradesInfoViewModel = new TradesViewModel
            {
                AvgNumberTradesPerMinute = info.Count / minutes,
                AvgTradeVolumePerMinute = info.Sum / (decimal)minutes,
            };


            return Page();
        }
    }
}