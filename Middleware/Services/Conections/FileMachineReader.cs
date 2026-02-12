using System.IO;

public class MachineReaderService
{
    public string Read(string fullFilePath)
    {
        // Leer todo el contenido
        string content = File.ReadAllText(fullFilePath);

        // Aquí podrías mover / archivar el archivo después de leerlo:
        // File.Move(fullFilePath, Path.Combine("Processed", Path.GetFileName(fullFilePath)));

        return content;
    }
}
