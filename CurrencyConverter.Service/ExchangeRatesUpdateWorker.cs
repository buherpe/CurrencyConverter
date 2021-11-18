using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Serilog;

namespace CurrencyConverter.Service
{
    public class ExchangeRatesUpdateWorker : BackgroundService
    {
        private readonly ILogger _log = Log.ForContext<ExchangeRatesUpdateWorker>();

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        private readonly IExchangeRateApiClient _exchangeRateApiClient;

        private readonly IExchangeRateApiWrapper _exchangeRateApiWrapper;

        public ExchangeRatesUpdateWorker(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration,
            IExchangeRateApiClient exchangeRateApiClient, IExchangeRateApiWrapper exchangeRateApiWrapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
            _exchangeRateApiClient = exchangeRateApiClient;
            _exchangeRateApiWrapper = exchangeRateApiWrapper;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

                    var request = context.Requests.FirstOrDefault(x => x.Id == 1) ?? new Request();

                    //_logger.LogInformation("Worker running at: {time}, context guid: {contextGuid}, request last update: {rlu}", DateTimeOffset.Now, context.Guid, request.LastUpdate);

                    var days = DateTime.UtcNow - request.LastUpdate;

                    if (days > TimeSpan.FromDays(1))
                    {
                        _log.Info($"Курс протух, обновляем");
                    
                        var response = await _exchangeRateApiWrapper.GetContentAsync(cancellationToken);

                        request.LastUpdate = DateTime.UtcNow;
                        request.Json = response;

                        context.Requests.Update(request);

                        await context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }

                await Task.Delay(5_000, cancellationToken);
            }
        }
    }
}