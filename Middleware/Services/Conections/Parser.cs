using System;
using System.Collections.Generic;
using Middleware.Models;

namespace Middleware.Services.Conections
{
    public static class Parser
    {
        public static List<LabResult> ParseASTM(string rawData)
        {
            var results = new List<LabResult>();

            if (string.IsNullOrWhiteSpace(rawData))
                return results;

            var lines = rawData.Split('\n');

            string currentSampleId = string.Empty;

            foreach (var line in lines)
            {
                var cleanLine = line.Trim();

                // Registro O → Información de muestra
                if (cleanLine.StartsWith("O|"))
                {
                    var fields = cleanLine.Split('|');
                    if (fields.Length > 2)
                        currentSampleId = fields[2];
                }

                // Registro R → Resultado
                if (cleanLine.StartsWith("R|"))
                {
                    var fields = cleanLine.Split('|');

                    if (fields.Length > 4)
                    {
                        var result = new LabResult
                        {
                            SampleId = currentSampleId,
                            TestCode = fields[2],
                            Value = fields[3],
                            Units = fields.Length > 4 ? fields[4] : "",
                            ReferenceRange = fields.Length > 5 ? fields[5] : "",
                            Flag = fields.Length > 6 ? fields[6] : ""
                        };

                        results.Add(result);
                    }
                }
            }

            return results;
        }
    }
}
