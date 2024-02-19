using Common.Db;
using Common.Db.Repositories;
using Common.Db.Repositories.Implementation;
using Common.Services;
using Common.Services.Implementation;
using Web.Services;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages(); 
            builder.Services.AddHttpClient();

            builder.Services.AddDbContext<TradesDbContext>(ServiceLifetime.Transient, ServiceLifetime.Transient);
            builder.Services.AddTransient<ITradesRepository, TradesRepository>();
            builder.Services.AddTransient<IIndependentReserveClient, IndependentReserveClient>();
            builder.Services.AddHostedService<BackgroundTradesService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
