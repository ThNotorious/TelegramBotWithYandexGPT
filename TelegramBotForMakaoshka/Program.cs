using Api;
using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TelegramBotForMakaoshka.Repositories.Context;
using TelegramBotForMakaoshka.Services;

internal class Program
{
    public static async Task Main()
    {
        DotEnv.Load();

        var logFilePath = Path.Combine("logs", "telegramBot.txt");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.Console()
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Minute)
            .CreateLogger();

        Log.Information("Программа запущена"); // Пример логирования информационного сообщения

        try
        {
            using IServiceScope scope = ServiceConfigurator.GetServicesScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            var connectionData = await context.ConnectionData.FirstAsync();

            string generatedText = await new YandexGptService(new HttpClient()).GenerateTextFromYandexGPT(connectionData);

            Console.WriteLine(generatedText);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
   
}

