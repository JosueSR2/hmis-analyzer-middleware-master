using System.Net.Sockets;
using System.Text;

public class TcpMachineReader
{
    private TcpClient _client;
    private NetworkStream _stream;

    public TcpMachineReader(string ip, int port)
    {
        _client = new TcpClient(ip, port);
        _stream = _client.GetStream();
    }

    public string ReadData()
    {
        byte[] buffer = new byte[1024];
        int bytesRead = _stream.Read(buffer, 0, buffer.Length);
        return Encoding.ASCII.GetString(buffer, 0, bytesRead);
    }

    public void Close()
    {
        _stream.Close();
        _client.Close();
    }
}
