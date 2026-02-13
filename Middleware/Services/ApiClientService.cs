using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Middleware.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;

        public ApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SendResultAsync(object payload)
        {
            try
            {
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/lab-results", content);

                if (response.IsSuccessStatusCode)
                    return true;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        Console.WriteLine("ERROR 404: Endpoint no encontrado en OpenELIS.");
                        break;

                    case HttpStatusCode.InternalServerError:
                        Console.WriteLine("ERROR 500: Error interno en OpenELIS.");
                        break;

                    default:
                        Console.WriteLine($"ERROR {(int)response.StatusCode}: {response.ReasonPhrase}");
                        break;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al enviar a OpenELIS: {ex.Message}");
                return false;
            }
        }
    }
}

