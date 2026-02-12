using System;
using System.IO;

public class FileMonitoringService
{
    private readonly string _watchFolder;
    private FileSystemWatcher _watcher;
    private readonly MachineReaderService _machineReader;
    private readonly AnalyzerService _analyzerService;

    public FileMonitoringService(
        string watchFolder,
        MachineReaderService machineReader,
        AnalyzerService analyzerService)
    {
        _watchFolder = watchFolder;
        _machineReader = machineReader;
        _analyzerService = analyzerService;
    }

    public void Start()
    {
        if (!Directory.Exists(_watchFolder))
            Directory.CreateDirectory(_watchFolder);

        _watcher = new FileSystemWatcher(_watchFolder);
        _watcher.Filter = "*.txt"; // Ajusta según extensión real
        _watcher.Created += OnNewFile;
        _watcher.EnableRaisingEvents = true;
    }

    private async void OnNewFile(object sender, FileSystemEventArgs e)
    {
        try
        {
            Console.WriteLine($"Nuevo archivo detectado: {e.FullPath}");

            // Asegura que el archivo esté listo para leer
            await Task.Delay(500);

            // Leer datos crudos
            string rawData = _machineReader.Read(e.FullPath);

            // Analizar y enviar
            await _analyzerService.Process(rawData);

            Console.WriteLine($"Archivo procesado: {e.Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error procesando archivo: {ex.Message}");
        }
    }
}
