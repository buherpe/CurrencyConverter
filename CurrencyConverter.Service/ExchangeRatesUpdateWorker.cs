using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace CurrencyConverter.Service
{
    public class ExchangeRatesUpdateWorker : BackgroundService
    {
        private readonly ILogger<ExchangeRatesUpdateWorker> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        private readonly IExchangeRateApiClient _exchangeRateApiClient;

        private readonly IExchangeRateApiWrapper _exchangeRateApiWrapper;

        public ExchangeRatesUpdateWorker(ILogger<ExchangeRatesUpdateWorker> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration,
            IExchangeRateApiClient exchangeRateApiClient, IExchangeRateApiWrapper exchangeRateApiWrapper)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
            _exchangeRateApiClient = exchangeRateApiClient;
            _exchangeRateApiWrapper = exchangeRateApiWrapper;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

                var request = context.Requests.FirstOrDefault(x => x.Id == 1) ?? new Request();

                //_logger.LogInformation("Worker running at: {time}, context guid: {contextGuid}, request last update: {rlu}", DateTimeOffset.Now, context.Guid, request.LastUpdate);

                var days = DateTime.Now - request.LastUpdate;

                if (days > TimeSpan.FromDays(1))
                {
                    _logger.LogInformation($"Курс протух, обновляем");
                    
                    var response = await _exchangeRateApiWrapper.GetContentAsync(cancellationToken);

                    request.LastUpdate = DateTime.Now;
                    request.Json = response;

                    await context.SaveChangesAsync(cancellationToken);
                }

                await Task.Delay(5_000, cancellationToken);
            }
        }
    }

    public class Rootobject
    {
        public string Result { get; set; }

        public string Documentation { get; set; }

        public string TermsOfUse { get; set; }

        public int TimeLastUpdateUnix { get; set; }

        public string TimeLastUpdateUtc { get; set; }

        public int TimeNextUpdateUnix { get; set; }

        public string TimeNextUpdateUtc { get; set; }

        public string BaseCode { get; set; }

        public Dictionary<string, decimal> ConversionRates { get; set; }
    }
}