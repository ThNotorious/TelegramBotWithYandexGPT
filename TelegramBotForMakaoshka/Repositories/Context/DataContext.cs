using Microsoft.EntityFrameworkCore;
using TelegramBotForMakaoshka.Models.Entities;

namespace TelegramBotForMakaoshka.Repositories.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<YandexGptConnectionDataModel> YandexGptConnectionData { get; set; }
        public DbSet<TelegramBotConnectionDataModel> TelegramBotConnectionData { get; set; }
    }
}
