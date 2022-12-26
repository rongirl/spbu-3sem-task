using System.Net;
using System.Net.Sockets;

namespace FTPServer;

public class Server
{ 
    private readonly TcpListener listener;
    private readonly CancellationTokenSource cancellationToken;
    private readonly List<Task> clients;
    public Server(IPAddress ip, int port)
    {
        listener = new TcpListener(ip, port);
        cancellationToken = new CancellationTokenSource();
        clients = new List<Task>(); 
       
    }

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

    public async Task Start()
    {
        listener.Start();
        Console.WriteLine($"Server started working...");
        while (!cancellationToken.IsCancellationRequested)
        {
            var socket = await listener.AcceptSocketAsync();
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

   public void Stop()
    {
        cancellationToken.Cancel();
    }
}