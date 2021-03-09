using System;
using System.IO;
using System.Linq;

namespace NamespaceFixer
{
    class Program
    {
        static int Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Please enter an existing path to your solution you want to analyze.");
                return 1;
            }

            var rootPath = args[0];

            if (!Directory.Exists(rootPath))
            {
                Console.WriteLine($"Path {rootPath} doesn't exist. ");
                return 1;
            }
            //var rootPath = @"C:\Users\serge\source\repos\WebApplication1";
            new ApplicationStarter().Start(rootPath);
            return 0; 
        }
    }
}
