using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;

        public ApiClientService()
        {
            _httpClient = new HttpClient();
        }

        public async Task SendResultsAsync(string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Cambia la URL por la de tu API real
            await _httpClient.PostAsync("http://localhost:5284/api/results", content);
        }
    }
}
