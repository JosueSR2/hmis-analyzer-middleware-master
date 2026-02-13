using Microsoft.AspNetCore.Mvc;
using Middleware.Services.Conections;

namespace Middleware.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyzerController : ControllerBase
    {
        private readonly AnalyzerService _analyzerService;

        public AnalyzerController(AnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [HttpGet("test")]
        public IActionResult Test() =>
            Ok("Middleware.API conectado y operativo ✅");

        [HttpPost("receive-result")]
        public async Task<IActionResult> ReceiveResult([FromBody] string rawData)
        {
            Console.WriteLine($"📥 ASTM recibido: {rawData}");

            bool success = await _analyzerService.Process(rawData);

            if (success)
                return Ok("Resultado procesado y enviado correctamente ✅");

            return StatusCode(500, "Error procesando resultado ❌");
        }
    }
}




