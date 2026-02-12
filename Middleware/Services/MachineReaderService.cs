using System;
using System.Collections.Generic;
using Middleware.Services.Conections;
using Middleware.Models;

namespace Middleware.Services.Conections
{
    public class MachineReaderService
    {
        public IEnumerable<LabResult> ReadMachineData(Machine machine)
        {
            List<LabResult> results = new List<LabResult>();

            if (machine.Mode == ModesOfCommunication.Off)
                return results;

            switch (machine.Protocol)
            {
                case CommunicateProtocol.Serial:

                    if (string.IsNullOrEmpty(machine.ComPort))
                        throw new Exception("ComPort no definido");

                    var serial = new SerialMachineReader(
                        machine.ComPort!,
                        machine.BaudRate,
                        (System.IO.Ports.Parity)machine.Parity);

                    string? serialData = serial.ReadLine();

                    if (!string.IsNullOrEmpty(serialData))
                        results.AddRange(Parser.ParseASTM(serialData));

                    serial.Close();
                    break;

                case CommunicateProtocol.TcpIp:

                    if (string.IsNullOrEmpty(machine.IpAddress))
                        throw new Exception("IpAddress no definido");

                    var tcp = new TcpMachineReader(machine.IpAddress!, machine.Port);

                    string? tcpData = tcp.ReadData();

                    if (!string.IsNullOrEmpty(tcpData))
                        results.AddRange(Parser.ParseASTM(tcpData));

                    tcp.Close();
                    break;

                case CommunicateProtocol.File:

                    if (string.IsNullOrEmpty(machine.FilePath))
                        throw new Exception("FilePath no definido para la máquina");

                    var fileReader = new FileMachineReader(machine.FilePath!);

                    foreach (var line in fileReader.ReadFiles())
                    {
                        if (!string.IsNullOrEmpty(line))
                            results.AddRange(Parser.ParseASTM(line));
                    }

                    break;
            }

            return results;
        }
    }
}




