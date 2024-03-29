using System.Threading;
using System.Threading.Tasks;
using Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Serilog;

namespace CurrencyConverter.Service
{
    public class PrometheusService : BackgroundService
    {
        private readonly ILogger _log = Log.ForContext<PrometheusService>();

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        private static readonly Counter TickTock = Metrics.CreateCounter("sampleapp_ticks_total", "Just keeps on ticking");

        public PrometheusService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var hostname = _configuration.GetSection("Prometheus:Hostname").Value;
            var port = int.Parse(_configuration.GetSection("Prometheus:Port").Value);

            _log.Info($"Hostname: {hostname}, port: {port}");
            
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