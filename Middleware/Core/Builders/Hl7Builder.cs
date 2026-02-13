using Middleware.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Middleware.Core.Builders
{
    public static class Hl7Builder
    {
        public static string Build(List<LabResult> results)
        {
            var sb = new StringBuilder();

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            sb.AppendLine($"MSH|^~\\&|Middleware|Lab|OpenELIS|Server|{timestamp}||ORU^R01|1234|P|2.3");
            sb.AppendLine($"PID|1||{results[0].SampleId}");
            sb.AppendLine($"OBR|1|||LAB^Laboratory");

            int index = 1;
            foreach (var result in results)
            {
                sb.AppendLine($"OBX|{index}|NM|{result.TestCode}^{result.TestName}||{result.Value}|{result.Units}|||F");
                index++;
            }

            return sb.ToString();
        }
    }
}
