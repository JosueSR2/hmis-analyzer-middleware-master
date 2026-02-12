using System.IO.Ports;

public class SerialMachineReader
{
    private SerialPort _port;

    public SerialMachineReader(string comPort, int baudRate, Parity parity)
    {
        _port = new SerialPort(comPort, baudRate, parity, 8, StopBits.One);
        _port.Open();
    }

    public string? ReadLine()
    {
        try
        {
            return _port.ReadLine(); // Lee hasta CR o LF
        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    public void Close() => _port.Close();
}
