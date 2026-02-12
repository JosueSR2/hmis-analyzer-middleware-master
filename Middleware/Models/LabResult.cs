namespace Middleware.Models
{
    public class LabResult
    {
        public string SampleId { get; set; }
        public string TestCode { get; set; }
        public string Value { get; set; }
        public string Units { get; set; }
        public string ReferenceRange { get; set; }
        public string AnalyzerName { get; set; }
        public DateTime ResultDate { get; set; }
    }
}
