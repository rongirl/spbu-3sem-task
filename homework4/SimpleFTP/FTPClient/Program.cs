using System.Net;

namespace FTPClient;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Enter ip and port, command, path . If you enter command 2 then enter the path to save the file.\n");
        if (args.Length < 4 || args.Length > 5)
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
            Console.WriteLine("Incorrect port");
            return;
        }
        var client = new Client(args[0], port);
        switch (args[2])
        {
            case "1":
            {
                var result = await client.List(args[3]);
                var size = result.Count;
                Console.WriteLine(size);
                foreach (var results in result)
                {
                    Console.WriteLine(results.Item1, results.Item2);
                }
                break;
            }
            case "2":
            {
                var size = await client.Get(args[3], args[4]);
                Console.WriteLine(size);
                Console.WriteLine("The file has been saved");
                break;
            }
            default:
            {
                Console.WriteLine("Incorrect input");
                break;
            }
        }
    }
}