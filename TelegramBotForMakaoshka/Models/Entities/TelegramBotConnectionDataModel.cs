using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramBotForMakaoshka.Models.Entities
{
    [Table("telegram_bot_connection_data")]
    public class TelegramBotConnectionDataModel
    {
        [Column("id")]
        public uint Id { get; set; }

        /// <summary>
        /// Токен для подключения к боту
        /// </summary>
        [Column("telegram_bot_token")]
        public required string TelegramBotToken { get; set; }
    }
}
