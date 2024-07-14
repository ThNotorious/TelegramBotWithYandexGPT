using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramBotForMakaoshka.Models.Entities
{
    [Table("yandex_gpt_connection_data")]
    public class YandexGptConnectionDataModel
    {
        [Column("id")]
        public uint Id { get; set; }

        /// <summary>
        /// url на сам gpt
        /// </summary>
        [Column("yandex_gpt_url")]
        public required string YandexGptUrl { get; set; }

        /// <summary>
        /// url используемой модели
        /// </summary>

        [Column("yandex_gpt_model_url")]
        public required string YandexGptModelUrl { get; set; }

        /// <summary>
        /// токен аутентификации
        /// </summary>

        [Column("yandex_gpt_oauth_token")]
        public required string YandexGptOauthToken { get; set; }


        /// <summary>
        /// Id папки
        /// </summary>
        [Column("yandex_gpt_folder_id")]
        public required string YandexGptFolderId { get; set; }

        /// <summary>
        /// url используемой модели
        /// </summary>

        [Column("yandex_gpt_token_url")]
        public required string YandexGptTokenUrl { get; set; }
    }
}
