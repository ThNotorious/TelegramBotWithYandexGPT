using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotForMakaoshka.Repositories.Context;

namespace TelegramBotForMakaoshka.Services
{
    public class TelegramBotService
    {
        private readonly YandexGptService _yandexGptService;
        private readonly DataContext _context;
        private readonly ITelegramBotClient _botClient;

        public TelegramBotService(YandexGptService yandexGptService, DataContext context)
        {
            _yandexGptService = yandexGptService;
            _context = context;

            var telegramBotConnectionData = _context.TelegramBotConnectionData.First();
            _botClient = new TelegramBotClient(telegramBotConnectionData.TelegramBotToken);

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message }
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: default
            );

            Console.WriteLine("Telegram Bot Service started...");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is Message message && message.Type == MessageType.Text)
            {
                string userMessage = message.Text;
                var userId = message.From.Id;
                var userName = message.From.Username ?? "неизвестный";

                // Вывод информации о пользователе
                Console.WriteLine($"ID пользователя: {userId}");
                Console.WriteLine($"Ник пользователя: {userName}");
                Console.WriteLine($"Сообщение пользователя: {userMessage}");

                try
                {
                    var yandexGptConnectionData = await _context.YandexGptConnectionData.FirstAsync(cancellationToken);
                    string responseMessage = await _yandexGptService.GenerateTextFromYandexGPT(yandexGptConnectionData, userMessage);
                    await _botClient.SendTextMessageAsync(message.Chat, responseMessage, cancellationToken: cancellationToken);

                    Console.WriteLine($"Ответ на сообщение: {responseMessage}");
                }
                catch (Exception ex)
                {
                    await _botClient.SendTextMessageAsync(message.Chat, $"Произошла ошибка: {ex.Message}", cancellationToken: cancellationToken);
                    Console.WriteLine($"Ошибка при обработке сообщения: {ex.Message}");
                }
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine($"Ошибка: {errorMessage}");
            return Task.CompletedTask;
        }
    }
}
