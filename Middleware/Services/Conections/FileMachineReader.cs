using System.Collections.Generic;
using System.IO;

namespace Middleware.Services.Conections
{
    public class FileMachineReader
    {
        private readonly string _path;

        public FileMachineReader(string path)
        {
            _path = path;
        }

        public IEnumerable<string> ReadFiles()
        {
            if (!Directory.Exists(_path))
                yield break;

            var files = Directory.GetFiles(_path, "*.txt");

            foreach (var file in files)
            {
                string content = File.ReadAllText(file);
                yield return content;

                // Opcional: borrar después de procesar
                File.Delete(file);
            }
        }
    }
}
