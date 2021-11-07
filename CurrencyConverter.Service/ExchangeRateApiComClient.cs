using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CurrencyConverter.Service
{
    public interface IExchangeRateApiClient
    {

    }

    public class ExchangeRateApiComClient : IExchangeRateApiClient
    {
        private readonly ILogger _log = Log.ForContext<ExchangeRateApiComClient>();

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        public ExchangeRateApiComClient(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }
    }
}