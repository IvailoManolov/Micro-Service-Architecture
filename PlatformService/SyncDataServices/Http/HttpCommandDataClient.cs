using System.Text.Json;
using WebApplication1.DTOs;

namespace WebApplication1.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                System.Text.Encoding.UTF8,
                "application/json"
                );

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}",httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to Command Service success!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to Command Service failed!");
            }
        }
    }
}
