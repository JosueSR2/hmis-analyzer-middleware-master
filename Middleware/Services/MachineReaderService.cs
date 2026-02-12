using System;
using System.Collections.Generic;
using Middleware; // <-- Asegúrate de usar el namespace correcto

namespace Middleware.Services
{
    public class MachineReaderService
    {
        public IEnumerable<string> ReadMachineData(Machine machine)
        {
            List<string> results = new List<string>();

            if(machine.Mode == ModesOfCommunication.Off)
                return results;

            switch(machine.Protocol)
            {
                case CommunicateProtocol.Serial:
                    var serial = new SerialMachineReader(machine.ComPort, machine.BaudRate, machine.Parity);
                    string serialData = serial.ReadLine();
                    results.AddRange(Parser.ParseASTM(serialData));
                    serial.Close();
                    break;

                case CommunicateProtocol.TcpIp:
                    var tcp = new TcpMachineReader(machine.IpAddress, machine.Port);
                    string tcpData = tcp.ReadData();
                    results.AddRange(Parser.ParseASTM(tcpData));
                    tcp.Close();
                    break;

                case CommunicateProtocol.File:
                    if(string.IsNullOrEmpty(machine.FilePath))
                        throw new Exception("FilePath no definido para la máquina");
                    var fileReader = new FileMachineReader(machine.FilePath);
                    foreach(var line in fileReader.ReadFiles())
                        results.AddRange(Parser.ParseASTM(line));
                    break;
            }

            return results;
        }
    }
}


