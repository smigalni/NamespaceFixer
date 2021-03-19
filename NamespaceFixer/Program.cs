using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NamespaceFixer
{
    class Program
    {
        static int Main(string[] args)
        {
            string rootPath;
            if (Debugger.IsAttached)
            {
                rootPath = @"C:\Users\serge\source\repos\WebApplication1";
            }
            else
            {
                if (!args.Any())
                {
                    Console.WriteLine("Please enter an existing path to your solution you want to analyze.");
                    return 1;
                }
                rootPath = args[0];

                if (!Directory.Exists(rootPath))
                {
                    Console.WriteLine($"Path {rootPath} doesn't exist. ");
                    return 1;
                }                
            }
           
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            new ApplicationStarter().Start(rootPath);

            stopWatch.Stop();
            Console.WriteLine($"Time Elapsed: {stopWatch.Elapsed}");
            return 0;
        }
    }
}
