using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Polly;
using Prometheus;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CurrencyConverter.Service
{
    public class TelegramBotWorker : BackgroundService
    {
        private readonly ILogger _log = Log.ForContext<TelegramBotWorker>();

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        private static readonly Histogram HandleUpdateAsyncDuration = Metrics.CreateHistogram("TelegramBotWorker_HandleUpdateAsyncDuration", "Histogram of login call processing durations.");

        public TelegramBotWorker(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                _log.Info($"Запускаем тг бота");

                var botClient = new TelegramBotClient(_configuration.GetSection("Telegram:Token").Value);

                var policy = Policy.Handle<Exception>().WaitAndRetryForeverAsync(retryAttempt =>
                {
                    var waitSeconds = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));

                    if (waitSeconds.TotalSeconds > 60)
                    {
                        waitSeconds = TimeSpan.FromSeconds(60);
                    }

                    _log.Info($"Попытка: {retryAttempt}, ждем {waitSeconds.TotalSeconds}с");
                    return waitSeconds;
                });

                var me = await policy.ExecuteAsync(async t => await botClient.GetMeAsync(cancellationToken), cancellationToken);

                //var me = await botClient.GetMeAsync(cancellationToken);

                _log.Info($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

                using var cts = new CancellationTokenSource();

                // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
                botClient.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), cancellationToken: cts.Token);
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _log.Error(exception, $"HandleErrorAsync");

            await Task.Delay(1000, cancellationToken);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            using var handleUpdateAsyncDurationTimer = HandleUpdateAsyncDuration.NewTimer();

            try
            {
                if (update.Type != UpdateType.Message)
                    return;
                if (update.Message.Type != MessageType.Text)
                    return;

                var chatId = update.Message.Chat.Id;

                _log.Info($"Чат: {chatId}, юзер: {update.Message.From.Id}, текст: {update.Message.Text}");

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

                        if (sum > 10)
                        {
                            sums.Add($"{Math.Round(sum, 0).ToString("N0", CultureInfo.InvariantCulture)}{symbol}");
                        }
                        else
                        {
                            sums.Add($"{Math.Round(sum, 2).ToString("N2", CultureInfo.InvariantCulture)}{symbol}");
                        }
                    }

                    str += $"{string.Join(" | ", sums)}\n";
                }

                await botClient.SendTextMessageAsync(chatId, $"{str}", cancellationToken: cancellationToken, replyToMessageId: update.Message.MessageId);
            }
            catch (Exception e)
            {
                _log.Error(e, $"Ошибка HandleUpdateAsync");
            }
        }
    }
}