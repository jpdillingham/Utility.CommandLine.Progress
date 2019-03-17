using System;
using System.Threading;
using Utility.CommandLine.ProgressBar;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var pb = new ProgressBar(width: -5, value: 0);

            for (int i = 0; i < 100; i++)
            {
                pb.PerformStep();
                Console.Write($"\r{pb}[{Math.Round(pb.Percent*100,1)}%]");
                Thread.Sleep(100);
            }
        }
    }
}
