using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.Service
{
    public interface IExchangeRateApiClient
    {

    }

    public class ExchangeRateApiComClient : IExchangeRateApiClient
    {
        private readonly ILogger<ExchangeRateApiComClient> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        public ExchangeRateApiComClient(ILogger<ExchangeRateApiComClient> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        public async Task Q()
        {

        }
    }
}