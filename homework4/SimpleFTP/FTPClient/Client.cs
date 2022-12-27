using System.Net.Sockets;

namespace FTPClient;

/// <summary>
/// Implementation of client
/// </summary>
public class Client
{
    private readonly string ip;
    private readonly int port;

    /// <summary>
    /// Initializes an Instance of the Client Class
    /// </summary>
    /// <param name="ip">Ip</param>
    /// <param name="port">Port</param>
    public Client(string ip, int port)
    {
        this.ip = ip;
        this.port = port;
    }

    /// <summary>
    /// Sends request for getting list of files and directories on server.
    /// </summary>
    /// <param name="path">Directory path</param>
    /// <returns>List (string, bool) where string is path 
    /// bool true if path is path to file</returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public async Task<List<(string, bool)>> List(string path)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(ip, port);
        var stream = client.GetStream();
        var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync("1" + " " + path);
        using var reader = new StreamReader(stream);
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

    /// <summary>
    /// Sends request for downloading file and getting it's size
    /// </summary>
    /// <param name="path">Path to get file</param>
    /// <param name="pathForSave">Path to save file</param>
    /// <returns></returns>
    public async Task<int> Get(string path, string pathForSave)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(ip, port);
        var stream = client.GetStream();
        var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync("2" + " " + path);
        using var reader = new StreamReader(stream);
        var size = await reader.ReadLineAsync();
        await using var file = File.Create(pathForSave);
        await stream.CopyToAsync(file);
        return Convert.ToInt32(size);
    }
}