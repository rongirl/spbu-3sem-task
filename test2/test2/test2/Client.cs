using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Chat;

public class Client
{
    private readonly TcpClient client;
    private NetworkStream stream;
    public Client(int port, string ip)
    {
        client = new TcpClient(ip, port);
    }

    public async Task Start()
    {
        await using (stream = client.GetStream())
        {
            Get();
            await Send();
        }
    }

    private void Get()
    {
        Task.Run(async () =>
        {
            using var reader = new StreamReader(stream);
            var message = await reader.ReadLineAsync();
            while (message != "exit")
            {
                Console.WriteLine(message);
                message = await reader.ReadLineAsync();
            }
            client.Close();
        });
    }

    private Task Send()
    {
        return Task.Run(async () =>
        {
            await using var writer = new StreamWriter(stream) { AutoFlush = true };
            var tasks = new List<Task>();
            var message = Console.ReadLine();
            while (message != "exit")
            {
                tasks.Add(writer.WriteLineAsync(message));
                message = Console.ReadLine();
            }
            Task.WaitAll(tasks.ToArray());
            client.Close();
        });
    }
}
