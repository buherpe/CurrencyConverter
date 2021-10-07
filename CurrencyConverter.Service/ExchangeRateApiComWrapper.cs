using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace CurrencyConverter.Service
{
    public interface IExchangeRateApiWrapper
    {
        Task<string> GetContentAsync(CancellationToken cancellationToken);
    }

    public class ExchangeRateApiComWrapper : IExchangeRateApiWrapper
    {
        private readonly ILogger<ExchangeRatesUpdateWorker> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        public ExchangeRateApiComWrapper(ILogger<ExchangeRatesUpdateWorker> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        public async Task<string> GetContentAsync(CancellationToken cancellationToken = default)
        {
            var client = new RestClient(_configuration.GetSection("ExchangeRateApi:Url").Value);

            client.UseNewtonsoftJson(new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
            });

            var apiRequest = new RestRequest();

            var response = await client.ExecuteGetAsync(apiRequest, cancellationToken);

            return response.Content;
        }
    }
}