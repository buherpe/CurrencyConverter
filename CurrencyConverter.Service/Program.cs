using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

    public class TelegramBotWorker : BackgroundService
    {
        private readonly ILogger<TelegramBotWorker> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        private bool _startup = true;

        public TelegramBotWorker(ILogger<TelegramBotWorker> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_startup)
                {
                    await StartTgBot();

                    _startup = false;
                }

                await Task.Delay(5_000, stoppingToken);
            }
        }

        public async Task StartTgBot()
        {
            _logger.LogInformation($"Запускаем тг бота");

            var botClient = new TelegramBotClient(_configuration.GetSection("Telegram:Token").Value);

            var me = await botClient.GetMeAsync();

            _logger.LogInformation($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            botClient.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), cts.Token);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation(errorMessage);

            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;

            _logger.LogInformation($"Received a '{update.Message.Text}' message in chat {chatId}.");

            var sumAndCurrencies = Helper.ParseSumAndCurrency(update.Message.Text);

            if (!sumAndCurrencies.Any())
            {
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            var request = context.Requests.FirstOrDefault(x => x.Id == 1) ?? new Request();

            //_logger.LogInformation($"{request.Json}");

            var str = $"";

            foreach (var sumAndCurrency in sumAndCurrencies)
            {
                var exchangeRate = JToken.Parse(request.Json)["conversion_rates"]["KZT"].Value<decimal>() / JToken.Parse(request.Json)["conversion_rates"]["RUB"].Value<decimal>();

                switch (sumAndCurrency.Currency)
                {
                    case Currency.None:
                        break;
                    case Currency.Rub:
                        var sum2 = sumAndCurrency.Sum * exchangeRate;
                        str += $"{sum2}\n";

                        break;
                    case Currency.Tenge:
                        var sum3 = sumAndCurrency.Sum / exchangeRate;
                        str += $"{sum3}\n";

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            await botClient.SendTextMessageAsync(chatId, $"{str}", cancellationToken: cancellationToken, replyToMessageId: update.Message.MessageId);
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