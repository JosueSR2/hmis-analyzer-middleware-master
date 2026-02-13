using Middleware.Core.Models;
using System;
using System.Collections.Generic;

namespace Middleware.Core.Parsers
{
    public class DimensionParser : IAnalyzerParser
    {
        public List<LabResult> Parse(string rawMessage)
        {
            var results = new List<LabResult>();

            // Eliminar STX y ETX
            rawMessage = rawMessage
                .Replace(((char)0x02).ToString(), "")
                .Replace(((char)0x03).ToString(), "");

            var fields = rawMessage.Split((char)0x1C);

            string sampleId = fields.Length > 2 ? fields[2] : "UNKNOWN";

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] == "NA" || fields[i] == "K" || fields[i] == "CL" || fields[i] == "CRE2")
                {
                    results.Add(new LabResult
                    {
                        SampleId = sampleId,
                        TestCode = fields[i],
                        TestName = fields[i],
                        Value = fields[i + 1],
                        Units = fields[i + 2],
                        Timestamp = DateTime.Now
                    });
                }
            }

            return results;
        }
    }
}
