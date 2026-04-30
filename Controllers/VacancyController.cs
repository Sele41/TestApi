using Microsoft.AspNetCore.Mvc;
using Nfc_Menu_Api.Modelss;
using System.Text.Json;
using Nfc_Menu_Api.Modelss;
using Nfc_Menu_Api.Services;

namespace TelegramVacancyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacancyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TelegramService _telegramService;

        private const string CompanyApiUrl =
            "https://jsonplaceholder.typicode.com/posts";

        public VacancyController(
            IHttpClientFactory httpClientFactory,
            TelegramService telegramService)
        {
            _httpClientFactory = httpClientFactory;
            _telegramService = telegramService;
        }

        [HttpGet("sync-vacancies")]
        public async Task<IActionResult> SyncVacancies()
        {
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                // STEP 1: Fetch data from external API
                var response = await httpClient.GetAsync(CompanyApiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest("Failed to fetch vacancies");
                }

                var json = await response.Content.ReadAsStringAsync();

                var vacancies = JsonSerializer.Deserialize<List<PostModel>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (vacancies == null || !vacancies.Any())
                {
                    return Ok("No vacancies found");
                }

                // STEP 2: Post first 5 to Telegram
               

                    await _telegramService.SendMessageAsync("Hi there");
                

                return Ok("Vacancies posted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}