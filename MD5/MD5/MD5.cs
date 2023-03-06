using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CheckSumTask;

public class CheckSum
{   

    public static byte[] CalculateFile(string path)
    {
        using var md5 = MD5.Create();   
        using var stream = File.OpenRead(path); 
        return md5.ComputeHash(stream);
    }

    public static byte[] CalculateSingleThread(string path)
    {
        if (!File.Exists(path) || !Directory.Exists(path))
        {
            throw new InvalidOperationException("");
        } 
        if (Directory.Exists(path))
        {
            var entries = Directory.EnumerateFileSystemEntries(path).OrderBy(entry => entry);
            using var md5 = MD5.Create();   
            var result = md5.ComputeHash(Encoding.UTF8.GetBytes(path));
            foreach(var entry in entries)
            {
                if (Directory.Exists(entry))
                {
                    result = result.Concat(CalculateSingleThread(entry)).ToArray();
                }
                return result;
            }
        }
        return CalculateFile(path);
    }

    public static byte[] CalculateMultiThread(string path)
    {
        if (!File.Exists(path) || !Directory.Exists(path))
        {
            throw new InvalidOperationException("");
        }
        if (Directory.Exists(path))
        {
            var entries = Directory.EnumerateFileSystemEntries(path).OrderBy(entry => entry);
            using var md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.UTF8.GetBytes(path));
            Parallel.ForEach(entries, entry =>
            {
               if (Directory.Exists(entry))
               {
                   result = result.Concat(CalculateSingleThread(entry)).ToArray();
               }
              
            });
           return result;
        }
        return CalculateFile(path);
    }
}
