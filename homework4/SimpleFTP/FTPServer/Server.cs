using System.Net;
using System.Net.Sockets;

namespace FTPServer;

/// <summary>
/// Implementation of server
/// </summary>
public class Server
{ 
    private readonly TcpListener listener;
    private readonly CancellationTokenSource cancellationToken;
    private readonly List<Task> clients;

    /// <summary>
    /// nitializes an Instance of the Server Class
    /// </summary>
    /// <param name="port">Port</param>
    public Server(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        cancellationToken = new CancellationTokenSource();
        clients = new List<Task>();
    }

    /// <summary>
    /// Performs Get command
    /// </summary>
    private static async Task Get(StreamWriter writer, string path)
    {
        if (!File.Exists(path))
        {
            await writer.WriteAsync("-1");
            return;
        }
        var size = new FileInfo(path).Length;
        await writer.WriteAsync($"{size} ");
        var reader = File.OpenRead(path);
        await reader.CopyToAsync(writer.BaseStream);
    }

    /// <summary>
    /// Performs List command
    /// </summary>
    private static async Task List(StreamWriter writer, string path)
    {
        if (!Directory.Exists(path))
        {
            await writer.WriteAsync("-1");
            return;
        }
        var directories = Directory.GetDirectories(path);   
        var files = Directory.GetFiles(path);
        var size = files.Length + directories.Length;
        var result = size.ToString();
        foreach (var directory in directories)
        {
            result += $" {directory} true";
        }
        foreach (var file in files)
        {
            result += $" {file} false";
        }
       await writer.WriteAsync(result);
    }

    /// <summary>
    /// Starts working
    /// </summary>
    /// <returns></returns>
    public async Task Start()
    {
        listener.Start();
        Console.WriteLine($"Server started working...");
        while (!cancellationToken.IsCancellationRequested)
        {
           using var socket = await listener.AcceptSocketAsync(cancellationToken.Token);
            var client = Task.Run(async () => {
                var stream = new NetworkStream(socket);
                var reader = new StreamReader(stream);
                var data = (await reader.ReadLineAsync())?.Split(' ');
                var writer = new StreamWriter(stream) { AutoFlush = true };
                if (data != null)
                {
                    if (data[0] == "1")
                    {
                        await List(writer, data[1]);
                    }
                    else if (data[0] == "2")
                    {
                        await Get(writer, data[1]);
                    }
                }
                socket.Close();
            });
            clients.Add(client);
        }
        Task.WaitAll(clients.ToArray());
    }

    /// <summary>
    /// Stop working
    /// </summary>
   public void Stop()
    {
        cancellationToken.Cancel();
    }
}