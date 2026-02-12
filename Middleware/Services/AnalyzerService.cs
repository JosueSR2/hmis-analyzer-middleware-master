using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Middleware.Encoders;
using Middleware.Models;
using Middleware.Services.Conections;

namespace Middleware.Services
{
    public class AnalyzerService
    {
        private readonly JsonEncoder _jsonEncoder;
        private readonly HL7Encoder _hl7Encoder;
        private readonly ApiClientService _apiClient;

        public AnalyzerService(
            JsonEncoder jsonEncoder,
            HL7Encoder hl7Encoder,
            ApiClientService apiClient)
        {
            _jsonEncoder = jsonEncoder;
            _hl7Encoder = hl7Encoder;
            _apiClient = apiClient;
        }

        public async Task Process(string rawData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rawData))
                {
                    Console.WriteLine("Datos vacíos recibidos.");
                    return;
                }

                // Parsear datos ASTM
                List<LabResult> results = Parser.ParseASTM(rawData);

                if (results == null || results.Count == 0)
                {
                    Console.WriteLine("No se encontraron resultados válidos.");
                    return;
                }

                Console.WriteLine($"Se encontraron {results.Count} resultados.");

                // Convertir a JSON
                string jsonPayload = _jsonEncoder.Encode(results);

                // (Opcional) Generar HL7
                string hl7Payload = string.Empty;
                foreach (var result in results)
                {
                    hl7Payload += _hl7Encoder.Encode(result) + "\r\n";
                }

                // Enviar a API
                await _apiClient.SendResultsAsync(jsonPayload);

                Console.WriteLine("Datos enviados correctamente a la API.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AnalyzerService: {ex.Message}");
            }
        }
    }
}



