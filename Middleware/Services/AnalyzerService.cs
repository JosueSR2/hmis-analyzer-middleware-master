using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AnalyzerService
{
    private readonly Parser _parser;
    private readonly JsonEncoder _jsonEncoder;
    private readonly HL7Encoder _hl7Encoder;
    private readonly ApiClientService _apiClient;

    public AnalyzerService(
        Parser parser,
        JsonEncoder jsonEncoder,
        HL7Encoder hl7Encoder,
        ApiClientService apiClient)
    {
        _parser = parser;
        _jsonEncoder = jsonEncoder;
        _hl7Encoder = hl7Encoder;
        _apiClient = apiClient;
    }

    public async Task Process(string rawData)
    {
        // 1) Parsear a resultados
        List<LabResult> results = _parser.Parse(rawData);

        if (results == null || results.Count == 0)
        {
            Console.WriteLine("No se encontraron resultados válidos.");
            return;
        }

        // 2) Convertir a JSON
        string jsonPayload = _jsonEncoder.Encode(results);

        // 3) Convertir a HL7 si necesitas HL7 también
        string hl7Payload = null;
        foreach (var r in results)
        {
            hl7Payload += _hl7Encoder.Encode(r) + "\r\n";
        }

        // 4) Enviar JSON por API
        await _apiClient.SendResultsAsync(jsonPayload);

        Console.WriteLine("Datos enviados a API correctamente.");
    }
}

