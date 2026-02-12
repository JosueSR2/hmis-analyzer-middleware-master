using System;
using System.Collections.Generic;

namespace Middleware
{
    public class Machine
    {
        public string Name { get; set; }
        public CommunicateProtocol Protocol { get; set; }
        public ModesOfCommunication Mode { get; set; }
        public Analyzer AnalyzerType { get; set; }

        // TCP/IP
        public string? IpAddress { get; set; }
        public int Port { get; set; }
        public string? FilePath { get; set; }

        // Serial
        public string? ComPort { get; set; }
        public int BaudRate { get; set; }
        public CommunicationParameterParity Parity { get; set; }

        public Machine(string name, Analyzer analyzer, CommunicateProtocol protocol, ModesOfCommunication mode)
        {
            Name = name;
            AnalyzerType = analyzer;
            Protocol = protocol;
            Mode = mode;
        }

        // Simula lectura de datos según modo y protocolo
        public IEnumerable<string> ReadData()
        {
            List<string> results = new List<string>();

            if (Mode == ModesOfCommunication.Off)
                return results;

            // Aquí es donde se implementaría la lógica real de comunicación
            if (Protocol == CommunicateProtocol.TcpIp)
            {
                results.Add($"[{Name}] Datos leídos por TCP/IP");
            }
            else if (Protocol == CommunicateProtocol.Serial)
            {
                results.Add($"[{Name}] Datos leídos por Serial");
            }

            // Ejemplo simulado de datos de la máquina
            results.Add($"{Name}|Glucosa|{RandomValue(70, 120)}");
            results.Add($"{Name}|Hemoglobina|{RandomValue(12, 16)}");

            return results;
        }

        private double RandomValue(int min, int max)
        {
            Random rnd = new Random();
            return Math.Round(min + rnd.NextDouble() * (max - min), 2);
        }
    }
}
