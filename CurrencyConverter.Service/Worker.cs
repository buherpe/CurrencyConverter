using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyConverter.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private IClass1 _class1;

        public Worker(ILogger<Worker> logger, IClass1 class1)
        {
            _logger = logger;
            _class1 = class1;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}, class1 guid: {class1guid}", DateTimeOffset.Now, _class1.Guid);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
