using Microsoft.EntityFrameworkCore;
using TelegramBotForMakaoshka.Models.Entities;

namespace TelegramBotForMakaoshka.Repositories.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<ConnectionDataModel> ConnectionData { get; set; }
    }
}
