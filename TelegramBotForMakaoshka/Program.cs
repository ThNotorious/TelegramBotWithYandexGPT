using Api;
using Microsoft.Extensions.DependencyInjection;
using TelegramBotForMakaoshka.Services;

var serviceScope = ServiceConfigurator.GetServicesScope();
_ = serviceScope.ServiceProvider.GetRequiredService<TelegramBotService>();

Console.WriteLine("Бот запущен. Нажмите любую клавишу для завершения...");
Console.ReadKey();

serviceScope.Dispose();
