using System.Net;

namespace FTPServer;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Enter port.\n");
        if (args.Length != 1)
        {
            Console.WriteLine("Incorrect input");
            return;
        }
        if (!int.TryParse(args[0], out int port))
        {
            Console.WriteLine(args[1]);
            return;
        }
        var server = new Server(port);
        await server.Start();
    }
}
