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
                Directory.CreateDirectory(_watchFolder);

            _watcher = new FileSystemWatcher
            {
                Path = _watchFolder,
                Filter = "*.txt",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            _watcher.Created += async (s, e) => await ProcessFile(e.FullPath);
            _watcher.EnableRaisingEvents = true;

            Console.WriteLine($"Monitoreando carpeta: {_watchFolder}");
        }

        private async Task ProcessFile(string filePath)
        {
            try
            {
                await WaitForFileReady(filePath);

                Console.WriteLine($"Archivo detectado: {Path.GetFileName(filePath)}");

                string rawData = await File.ReadAllTextAsync(filePath);

                bool success = await _analyzerService.Process(rawData);

                if (success)
                {
                    File.Delete(filePath);
                    Console.WriteLine("Archivo procesado y eliminado correctamente.");
                }
                else
                {
                    MoveToErrorFolder(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando archivo: {ex.Message}");
                MoveToErrorFolder(filePath);
            }
        }

        private async Task WaitForFileReady(string filePath)
        {
            while (true)
            {
                try
                {
                    using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                    break;
                }
                catch (IOException)
                {
                    await Task.Delay(500);
                }
            }
        }

        private void MoveToErrorFolder(string filePath)
        {
            try
            {
                string errorFolder = Path.Combine(Path.GetDirectoryName(filePath)!, "Error");

                if (!Directory.Exists(errorFolder))
                    Directory.CreateDirectory(errorFolder);

                string newPath = Path.Combine(errorFolder, Path.GetFileName(filePath));

                if (File.Exists(newPath))
                    File.Delete(newPath);

                File.Move(filePath, newPath);

                Console.WriteLine("Archivo movido a carpeta Error.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo mover el archivo a Error: {ex.Message}");
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
