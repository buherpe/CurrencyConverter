using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace CurrencyConverter.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<MyDbContext>(options => options
                        .UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection"))
                        .UseSnakeCaseNamingConvention());

                    services.AddHostedService<ExchangeRatesUpdateWorker>();
                    services.AddHostedService<TelegramBotWorker>();
                    services.AddSingleton<IClass1, Class1>();
                    services.AddSingleton<IExchangeRateApiClient, ExchangeRateApiComClient>();
                    services.AddSingleton<IExchangeRateApiWrapper, ExchangeRateApiComWrapper>();
                });
    }

    public interface IClass1
    {
        Guid Guid { get; set; }
    }

    public class Class1 : IClass1
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
    }

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

    public interface IExchangeRateApiClient
    {

    }

    public class ExchangeRateApiComClient : IExchangeRateApiClient
    {
        private readonly ILogger<ExchangeRatesUpdateWorker> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        public ExchangeRateApiComClient(ILogger<ExchangeRatesUpdateWorker> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        public async Task Q()
        {

        }
    }

    public class Request
    {
        public int Id { get; set; }

        public DateTime LastUpdate { get; set; }

        public string Json { get; set; }
    }

    public class MyDbContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public Guid Guid { get; set; } = Guid.NewGuid();

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
    }
}