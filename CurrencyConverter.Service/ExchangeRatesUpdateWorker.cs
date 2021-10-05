using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public ExchangeRatesUpdateWorker(ILogger<ExchangeRatesUpdateWorker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

                var request = context.Requests.FirstOrDefault(x => x.Id == 1) ?? new Request();

                //_logger.LogInformation("Worker running at: {time}, context guid: {contextGuid}, request last update: {rlu}", DateTimeOffset.Now, context.Guid, request.LastUpdate);

                var days = DateTime.Now - request.LastUpdate;

                //_logger.LogInformation($"days: {days}");

                if (days > TimeSpan.FromDays(1))
                {
                    _logger.LogInformation($"Курс протух, обновляем");

                    var client = new RestClient("https://v6.exchangerate-api.com/v6/f4b04e54320a07ff8fad04b4/latest/");

                    client.UseNewtonsoftJson(new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        },
                    });

                    var apiRequest = new RestRequest("RUB", DataFormat.Json);

                    var response = await client.ExecuteGetAsync(apiRequest, stoppingToken);

                    request.LastUpdate = DateTime.Now;
                    request.Json = response.Content;

                    await context.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(1_000, stoppingToken);
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