using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CurrencyConverter.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<MyDbContext>(options => options
                        .UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection"))
                        .UseSnakeCaseNamingConvention());

                    services.AddHostedService<ExchangeRatesUpdateWorker>();
                    services.AddHostedService<TelegramBotWorker>();
                    services.AddSingleton<IExchangeRateApiClient, ExchangeRateApiComClient>();
                    services.AddSingleton<IExchangeRateApiWrapper, ExchangeRateApiComWrapper>();
                });
    }

    public class Request
    {
        public int Id { get; set; }

        public DateTime LastUpdate { get; set; }

        public string Json { get; set; }
    }

    public class MyDbContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public Guid Guid { get; set; } = Guid.NewGuid();

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
    }
}