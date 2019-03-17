using System;
using System.Threading;
using Utility.CommandLine.ProgressBar;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            FullWidth();
            FitToWidth();
            FixedWidth();
            Disappear();
        }

        static void Loop(ProgressBar pb, Action<ProgressBar> action)
        {
            for (int i = 0; i < 100; i++)
            {
                pb.PerformStep();
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                action(pb);
                Thread.Sleep(10);
            }

            Console.Write("\n");
        }

        static void FullWidth()
        {
            var pb = new ProgressBar();

            for (int i = 0; i < 100; i++)
            {
                pb.PerformStep();
                Console.Write($"\r{pb}");
                Thread.Sleep(10);
            }

            Console.Write("\n");
        }

        static void FitToWidth()
        {
            var text = "Performing some background operation"; 

            var pb = new ProgressBar(width: 0 - (text.Length + 8), value: 0);

            for (int i = 0; i < 100; i++)
            {
                pb.PerformStep();
                Console.Write($"\r{text} [{Math.Round(pb.Percent * 100).ToString().PadLeft(3)}%] {pb}");
                Thread.Sleep(10);
            }

            Console.Write("\n");
        }

        static void FixedWidth()
        {
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>')), pb => Console.Write($"\rDoing something... {pb} Some other text."));
        }

        static void Disappear()
        {
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>', showWhenComplete: false)), pb => Console.Write($"\rDoing something... {pb} Some other text."));
        }
    }
}
