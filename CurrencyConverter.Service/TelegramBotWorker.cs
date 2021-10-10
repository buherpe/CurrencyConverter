using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CurrencyConverter.Service
{
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
            _logger.LogInformation($"��������� �� ����");

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
            try
            {
                if (update.Type != UpdateType.Message)
                    return;
                if (update.Message.Type != MessageType.Text)
                    return;

                var chatId = update.Message.Chat.Id;

                _logger.LogInformation($"���: {chatId}, ����: {update.Message.From.Id}, �����: {update.Message.Text}");

                var sumAndCurrencies = Helper.ParseSumAndCurrency(update.Message.Text);

                if (!sumAndCurrencies.Any())
                {
                    return;
                }

                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

                var request = context.Requests.FirstOrDefault(x => x.Id == 1) ?? new Request();

                var str = $"";

                foreach (var sumAndCurrency in sumAndCurrencies)
                {
                    var groupSetting = Helper.GetGroupSetting(update.Message.Chat.Id);
                    var targetCurrencies = groupSetting.GetTargetCurrencies(sumAndCurrency.Currency);
                    var sums = new List<string>();

                    foreach (var targetCurrency in targetCurrencies)
                    {
                        var source1 = JToken.Parse(request.Json)["conversion_rates"][Helper.CurrencySettings[sumAndCurrency.Currency].ISO].Value<decimal>();
                        var target1 = JToken.Parse(request.Json)["conversion_rates"][Helper.CurrencySettings[targetCurrency].ISO].Value<decimal>();
                        var exchangeRate2 = target1 / source1;

                        var sum = sumAndCurrency.Sum * exchangeRate2;
                        var symbol = Helper.CurrencySettings[targetCurrency].Symbols.FirstOrDefault();

                        sums.Add($"{Math.Round(sum, 0):N0}{symbol}");
                    }

                    str += $"{string.Join(" | ", sums)}\n";
                }

                await botClient.SendTextMessageAsync(chatId, $"{str}", cancellationToken: cancellationToken, replyToMessageId: update.Message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"������ HandleUpdateAsync");
            }
        }
    }
}