using System.Collections.Generic;
using System.Text.Json;
using Middleware.Models;

namespace Middleware.Encoders
{
    public class JsonEncoder
    {
        public string Encode(List<LabResult> results)
        {
            return JsonSerializer.Serialize(results);
        }
    }
}
