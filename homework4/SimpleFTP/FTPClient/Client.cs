using System.Net.Sockets;

namespace FTPClient;

public class Client
{
    private readonly string ip;
    private readonly int port;

    public Client(string ip, int port)
    {
        this.ip = ip;
        this.port = port;
    }

    public async Task<List<(string, bool)>> List(string path)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(ip, port);
        var stream = client.GetStream();
        var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync("1" + " " + path);
        var reader = new StreamReader(stream);
        var data = await reader.ReadToEndAsync();
        if (data == "-1")
        {
            throw new DirectoryNotFoundException();
        }
        var dataSplit = data.Split(' ');
        var result = new List<(string, bool)>();
        for (var i = 1; i < dataSplit.Length; i += 2)
        {
            result.Add((dataSplit[i], Convert.ToBoolean(dataSplit[i + 1])));
        }

        return result;
    }


    public async Task<int> Get(string path, string pathForSave)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(ip, port);
        var stream = client.GetStream();
        var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync("2" + " " + path);
        var reader = new StreamReader(stream);
        var size = await reader.ReadLineAsync();
        await using var file = File.Create(pathForSave);
        await stream.CopyToAsync(file);
        return Convert.ToInt32(size);
    }
}