using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using TelegramBotForMakaoshka.Repositories.Context;
using TelegramBotForMakaoshka.Services;

namespace Api
{
    public static class ServiceConfigurator
    {
        private static void Configure(IServiceCollection services)
        {
            MariaDbServerVersion serverVersion = new(new Version(10, 5, 21));

            // Прямая строка подключения
            var connectionString = "Server=localhost;Port=3306;Database=telegram_bot;User=dev;Password=Goldbergbill8;";

            services.AddDbContext<DataContext>(dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion)
                .LogTo(Log.Error, LogLevel.Error)
#if DEBUG
                .EnableSensitiveDataLogging()
#endif
                .EnableDetailedErrors());

            services.AddScoped<YandexGptService>();
        }

        /// <summary>
        /// Получение области применения сервисов
        /// </summary>
        /// <returns></returns>
        public static IServiceScope GetServicesScope()
        {
            var services = new ServiceCollection();
            Configure(services);
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.CreateScope();
        }
    }
}
