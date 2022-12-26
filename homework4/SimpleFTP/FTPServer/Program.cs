using System.Net;

namespace FTPServer;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Enter ip and port.\n");
        if (args.Length != 2)
        {
            Console.WriteLine("Incorrect input");
            return;
        }
        if (!IPAddress.TryParse(args[0], out IPAddress? address))
        {
            Console.WriteLine("Incorrect ip");
            return;
        }
        if (!int.TryParse(args[1], out int port))
        {
            Console.WriteLine(args[1]);
            return;
        }
        var server = new Server(address, port);
        await server.Start();
    }
}
