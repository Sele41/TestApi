using System.Text;
using System.Text.Json;

namespace Nfc_Menu_Api.Services
{
    public class TelegramService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TelegramService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendMessageAsync(string message)
        {
            var client = _httpClientFactory.CreateClient();

            var botToken = "8709404126:AAG7yBod245viNYSp1idu0WjGHK8FLQwzWE";
            var chatId = "@testchannel2141";

            var url = $"https://api.telegram.org/bot{botToken}/sendMessage";

            var payload = new
            {
                chat_id = chatId,
                text = message,
                parse_mode = "HTML"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            await client.PostAsync(url, content);
        }
    }
}