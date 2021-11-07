using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Serilog;

namespace CurrencyConverter.Service
{
    public interface IExchangeRateApiWrapper
    {
        Task<string> GetContentAsync(CancellationToken cancellationToken);
    }

    public class ExchangeRateApiComWrapper : IExchangeRateApiWrapper
    {
        private readonly ILogger _log = Log.ForContext<ExchangeRateApiComWrapper>();

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        public ExchangeRateApiComWrapper(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
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