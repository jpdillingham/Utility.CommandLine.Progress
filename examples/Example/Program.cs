using System;
using System.Threading;
using Utility.CommandLine.ProgressBar;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //FullWidth();
            //FitToWidth();
            //FixedWidth();
            //Disappear();
            WithSpinner();
            BlockySpinner();
        }

        static void Loop(ProgressBar pb, ProgressSpinner ps, Action<ProgressBar, ProgressSpinner> action)
        {
            for (int i = 0; i < 100; i++)
            {
                pb.PerformStep();
                //Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                action(pb, ps);
                Thread.Sleep(50);
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
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>')), null, (pb, ps) => Console.Write($"\rDoing something... {pb} Some other text."));
        }

        static void Disappear()
        {
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>', showWhenComplete: false)), null, (pb, ps) => Console.Write($"\rDoing something... {pb} Some other text."));
        }

        static void WithSpinner()
        {
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>')), new ProgressSpinner(), (pb, ps) => Console.Write($"\rDoing something... {pb} {ps}"));
        }

        static void BlockySpinner()
        {
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>')), new ProgressSpinner('█', '▓', '▒', '░', '▒', '▓'), (pb, ps) => Console.Write($"\rDoing something... {pb} {ps}"));
        }
    }
}
