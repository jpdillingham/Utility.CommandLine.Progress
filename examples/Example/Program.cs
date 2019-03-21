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
            //WithSpinner();
            //BlockySpinner();
            //BlockySpinner2();
            Marquee();
        }

        static void Loop(ProgressBar pb, Spinner ps, Marquee m, Action<ProgressBar, Spinner, Marquee> action)
        {
            for (int i = 0; i < pb.Maximum; i++)
            {
                pb.Increment();
                action(pb, ps, m);
                //Console.Write("\n");
                Thread.Sleep(25);
            }

            Console.Write("\n");
        }

        static void FullWidth()
        {
            var pb = new ProgressBar();

            for (int i = 0; i < 100; i++)
            {
                pb.Increment();
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
                pb.Increment();
                Console.Write($"\r{text} [{Math.Round(pb.Percent * 100).ToString().PadLeft(3)}%] {pb}");
                Thread.Sleep(5);
            }

            Console.Write("\n");
        }

        static void FixedWidth()
        {
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>')), null, null, (pb, ps, _) => Console.Write($"\rDoing something... {pb} Some other text."));
        }

        static void Disappear()
        {
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>', paddingLeft: 1)), null, null, (pb, ps, _) => Console.Write($"\rDoing something...{(pb.Complete ? pb.ToString() : string.Empty)} Some other text.".PadRight(Console.WindowWidth - 1)));
        }

        static void WithSpinner()
        {
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>')), new Spinner(), null, (pb, ps, _) => Console.Write($"\rDoing something... {pb} {ps}"));
        }

        static void BlockySpinner()
        {
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>')), new Spinner('█', '▓', '▒', '░', '▒', '▓'), null, (pb, ps, _) => Console.Write($"\rDoing something... {pb} {ps}"));
        }

        static void BlockySpinner2()
        {
            Loop(new ProgressBar(-30, format: new ProgressBarFormat(full: '█', empty: '░')), new Spinner('▀', '▄'), null, (pb, ps, _) => 
            {
                ps.Spin();
                Console.Write($"\rDoing something... {pb} {ps}".PadRight(Console.WindowWidth - 1));
            });
        }

        static void Marquee()
        {
            Loop(new ProgressBar(50, 0, 500, 1, 0), new Spinner(), new Marquee("█▓▒░", 20, new MarqueeFormat(leftToRight: false, bounce: true, reverseTextOnBounce: true, gap: null)), (pb, ps, m) =>
            {
                m.Scroll();
                ps.Spin();
                Console.Write($"\r{pb} {m} {ps}");
            });
            //Loop(new ProgressBar(10, 0, 1000, 1, 0), null, new Marquee("Hello, World!", 30, new MarqueeFormat(leftToRight: false, bounce: true, reverseTextOnBounce: false, gap:0)), (pb, ps, m) =>
            //{
            //    Console.Write($"\r{pb} {m}");
            //    m.PerformStep();
            //});
        }
    }
}
