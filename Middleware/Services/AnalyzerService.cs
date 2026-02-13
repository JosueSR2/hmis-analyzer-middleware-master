using System;
using System.Threading.Tasks;

namespace Middleware.Services.Conections
{
    public class AnalyzerService
    {
        private readonly ApiClientService _apiClient;

        public AnalyzerService(ApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> Process(string rawData)
        {
            try
            {
                var parsedResults = Parser.ParseASTM(rawData);

                foreach (var result in parsedResults)
                {
                    bool success = await _apiClient.SendResultAsync(result);

                    if (!success)
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AnalyzerService: {ex.Message}");
                return false;
            }
        }
    }
}




