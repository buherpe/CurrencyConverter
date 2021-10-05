using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private IClass1 _class1;

        public Worker(ILogger<Worker> logger, IClass1 class1, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _class1 = class1;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

                var request = context.Requests.FirstOrDefault(x => x.Id == 1);

                _logger.LogInformation("Worker running at: {time}, context guid: {contextGuid}, request last update: {rlu}", DateTimeOffset.Now, context.Guid, request.LastUpdate);

                var days = DateTime.Now - request.LastUpdate;

                _logger.LogInformation($"days: {days}");

                if (days > TimeSpan.FromDays(1))
                {

                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
