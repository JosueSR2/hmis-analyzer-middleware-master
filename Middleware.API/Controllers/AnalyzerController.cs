using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Middleware.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyzerController : ControllerBase
    {
        private static readonly ConcurrentQueue<string> ResultsQueue = new();

        [HttpGet("test")]
        public IActionResult Test() =>
            Ok("Middleware.API conectado y operativo ✅");

        [HttpPost("send-command")]
        public IActionResult SendCommand([FromBody] string command)
        {
            Console.WriteLine($"📤 Comando recibido: {command}");
            return Ok($"Comando '{command}' enviado al analizador");
        }

        [HttpPost("receive-result")]
        public IActionResult ReceiveResult([FromBody] string result)
        {
            ResultsQueue.Enqueue(result);
            Console.WriteLine($"📥 Resultado recibido: {result}");
            return Ok("Resultado almacenado");
        }

        [HttpGet("get-results")]
        public IActionResult GetResults()
        {
            var results = ResultsQueue.ToArray();
            ResultsQueue.Clear();
            return Ok(results);
        }
    }
}



