using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CurrencyConverter.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Serilog.Log.Logger = Helpers.Helper.GetDefaultLoggerConfig();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<MyDbContext>(options => options
                        .UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    services.AddHostedService<PrometheusService>();
                    services.AddHostedService<ExchangeRatesUpdateWorker>();
                    services.AddHostedService<TelegramBotWorker>();
                    services.AddSingleton<IExchangeRateApiClient, ExchangeRateApiComClient>();
                    services.AddSingleton<IExchangeRateApiWrapper, ExchangeRateApiComWrapper>();
                });
    }
}