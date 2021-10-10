using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Prometheus;

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

                    services.AddHostedService<PrometheusService>();
                    services.AddHostedService<ExchangeRatesUpdateWorker>();
                    services.AddHostedService<TelegramBotWorker>();
                    services.AddSingleton<IExchangeRateApiClient, ExchangeRateApiComClient>();
                    services.AddSingleton<IExchangeRateApiWrapper, ExchangeRateApiComWrapper>();

                });
    }

    public class PrometheusService : BackgroundService
    {
        private readonly ILogger<PrometheusService> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        private static readonly Counter TickTock = Metrics.CreateCounter("sampleapp_ticks_total", "Just keeps on ticking");

        public PrometheusService(ILogger<PrometheusService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var hostname = _configuration.GetSection("Prometheus:Hostname").Value;
            var port = int.Parse(_configuration.GetSection("Prometheus:Port").Value);

            _logger.LogInformation($"Hostname: {hostname}, port: {port}");

            var server = new MetricServer(hostname, port);
            server.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                TickTock.Inc();

                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}