using CheckSumTask;
using System.Diagnostics;

public class Program
{
    private static string ByteArrayToString(byte[] ba) => BitConverter.ToString(ba).Replace("-", "");

    public static void Main(string[] args)
    {;
        string path = Console.ReadLine();
        Console.WriteLine($"Path: {path}");
        Console.WriteLine("Check-sum: ");
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var singleThread = CheckSum.CalculateSingleThread(path);
        stopwatch.Stop();
        Console.WriteLine($"SingleThread result: {ByteArrayToString(singleThread)}");
        Console.WriteLine($"Time: {stopwatch.Elapsed}");
        Console.WriteLine();
        stopwatch = Stopwatch.StartNew();
        var multiThread = CheckSum.CalculateMultiThread(path);
        stopwatch.Stop();
        Console.WriteLine($"MultiThread result: {ByteArrayToString(multiThread)}");
        Console.WriteLine($"Time: {stopwatch.Elapsed}");
    }
}