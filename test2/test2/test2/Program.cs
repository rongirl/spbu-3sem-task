namespace Chat;
public class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 1)
        {
            int port = int.Parse(args[0]);
            var server = new Server(port);
            server.Start();
        }
        else if (args.Length == 2)
        {
            string ip = args[0];    
            int port = int.Parse(args[1]);
            var client = new Client(port,  ip);
            client.Start();
        }
    }

}