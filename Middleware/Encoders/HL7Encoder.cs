using System;
using Middleware.Models;

namespace Middleware.Encoders
{
    public class HL7Encoder
    {
        public string Encode(LabResult result)
        {
            return
                $"MSH|^~\\&|Middleware|Lab|OpenELIS|Server|{DateTime.Now:yyyyMMddHHmmss}||ORU^R01|1|P|2.3\r" +
                $"PID|||{result.SampleId}|||\r" +
                $"OBR|||{result.SampleId}||{result.TestCode}\r" +
                $"OBX|1|NM|{result.TestCode}||{result.Value}|{result.Units}|{result.ReferenceRange}|||F\r";
        }
    }
}
