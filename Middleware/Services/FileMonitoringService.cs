using System;
using System.IO;
using System.Threading.Tasks;

namespace Middleware.Services.Conections
{
    public class FileMonitoringService
    {
        private readonly string _watchFolder;
        private readonly AnalyzerService _analyzerService;
        private FileSystemWatcher? _watcher;

        public FileMonitoringService(string watchFolder, AnalyzerService analyzerService)
        {
            _watchFolder = watchFolder;
            _analyzerService = analyzerService;
        }

        public void Start()
        {
            if (!Directory.Exists(_watchFolder))
            {
                Directory.CreateDirectory(_watchFolder);
            }

            _watcher = new FileSystemWatcher
            {
                Path = _watchFolder,
                Filter = "*.txt", // puedes cambiar a *.dat si tu equipo usa ese formato
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            _watcher.Created += OnFileCreated;
            _watcher.EnableRaisingEvents = true;

            Console.WriteLine($"Monitoreando carpeta: {_watchFolder}");
        }

        private async void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                // Esperar un poco para asegurar que el archivo termine de copiarse
                await Task.Delay(500);

                string rawData = await File.ReadAllTextAsync(e.FullPath);

                Console.WriteLine($"Archivo detectado: {e.Name}");

                await _analyzerService.Process(rawData);

                // Opcional: eliminar archivo después de procesar
                File.Delete(e.FullPath);

                Console.WriteLine($"Archivo procesado y eliminado: {e.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando archivo {e.Name}: {ex.Message}");
            }
        }

        public void Stop()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
            }

            Console.WriteLine("Monitoreo detenido.");
        }
    }
}
