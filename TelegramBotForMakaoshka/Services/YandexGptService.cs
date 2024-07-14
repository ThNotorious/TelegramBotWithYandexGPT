using Newtonsoft.Json;
using System.Text;
using TelegramBotForMakaoshka.Models.Entities;

namespace TelegramBotForMakaoshka.Services
{
    public class YandexGptService(HttpClient client)
    {
        public async Task<string> GenerateTextFromYandexGPT(ConnectionDataModel connectionData)
        {
            var requestUrl = connectionData.YandexGptUrl;
            var requestData = new
            {
                modelUri = connectionData.YandexGptModelUrl,
                completionOptions = new
                {
                    stream = false,
                    temperature = 0.6,
                    maxTokens = 2000
                },
                messages = new[]
                {
                new { role = "system", text = "Скажи мне что-нибудь хорошее" }
            }
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            string iamToken = await GetIamToken(connectionData);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", iamToken);
            client.DefaultRequestHeaders.Add("x-folder-id", connectionData.YandexGptFolderId);

            HttpResponseMessage response = await client.PostAsync(requestUrl, requestContent);
            string responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response body: " + responseBody); // Для отладки

            dynamic responseJson = JsonConvert.DeserializeObject(responseBody);
            if (responseJson?.result == null || responseJson.result.alternatives.Count == 0)
            {
                throw new Exception("Некорректный ответ от Yandex GPT API.");
            }

            return responseJson.result.alternatives[0].message.text;
        }

        private async Task<string> GetIamToken(ConnectionDataModel connectionData)
        {
            var tokenData = new { yandexPassportOauthToken = connectionData.YandexGptOauthToken };
            var requestContent = new StringContent(JsonConvert.SerializeObject(tokenData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(connectionData.YandexGptTokenUrl, requestContent);
            string responseBody = await response.Content.ReadAsStringAsync();

            dynamic responseJson = JsonConvert.DeserializeObject(responseBody);
            if (responseJson?.iamToken == null)
            {
                throw new Exception("Не удалось получить IAM токен.");
            }

            return responseJson.iamToken;
        }

    }
}