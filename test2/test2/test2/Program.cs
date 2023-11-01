namespace Chat;
public class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 1)
        {
            int port = int.Parse(args[0]);
            var server = new Server(port);
            await server.Start();
        }
        else if (args.Length == 2)
        {
            string ip = args[0];    
            int port = int.Parse(args[1]);
            var client = new Client(port,  ip);
            await client.Start();
        }
    }

}