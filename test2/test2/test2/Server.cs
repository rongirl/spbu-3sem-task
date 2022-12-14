using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat;

public class Server
{
    private NetworkStream stream;
    private StreamReader reader;
    private StreamWriter writer;
    private readonly TcpListener listener;
    public Server(int port)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, port);
    }

    public async Task Start()
    {
        listener.Start();
        var client = await listener.AcceptTcpClientAsync();
        stream = client.GetStream();
        Get();
        await Send();
    } 

    private void Get()
    {
        Task.Run(async() =>
        {
           using (var reader = new StreamReader(stream))
           {
               var message = await reader.ReadLineAsync();
               while (message != "exit")
               {
                   Console.WriteLine(message);
                   message = await reader.ReadLineAsync();
               }
               listener.Stop();
           }
       });
    }

    private Task Send()
    {
        return Task.Run(async () =>
        {
            await using (var writer = new StreamWriter(stream) { AutoFlush = true })
            {
                var tasks = new List<Task>();
                var message = Console.ReadLine();
                while (message != "exit")
                {
                    tasks.Add(writer.WriteLineAsync(message));
                    message = Console.ReadLine();
                }
                Task.WaitAll(tasks.ToArray());
                listener.Stop();
            }
        });        
    }
}
