using System;
using System.Threading;
using Utility.CommandLine.Progress;

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

        static void Loop(ProgressBar pb, Spinner s, Marquee m, Action<ProgressBar, Spinner, Marquee> action)
        {
            for (int i = 0; i < pb.Maximum; i++)
            {
                pb?.Increment();
                s?.Spin();
                m?.Scroll();
                action(pb, s, m);
                //Console.Write("\n");
                Thread.Sleep(50);
            }

            Console.Write("\n");
        }

        static void FullWidth()
        {
            Loop(new ProgressBar(), null, null, (pb, s, m) =>
            {
                Console.Write($"\r{pb}");
            });
        }

        static void FitToWidth()
        {
            var text = "Performing some background operation"; 
            var pbar = new ProgressBar(width: 0 - (text.Length + 8), value: 0);

            Loop(pbar, null, null, (pb, s, m) =>
            {
                Console.Write($"\r{text} [{Math.Round(pb.Percent * 100).ToString().PadLeft(3)}%] {pb}");
            });
        }

        static void FixedWidth()
        {
            Loop(new ProgressBar(10, format: new ProgressBarFormat(full: '=', tip: '>')), null, null, (pb, ps, _) => Console.Write($"\rDoing something... {pb} Some other text."));
        }

        static void Disappear()
        {
            ProgressBar pbar = null;
            var fmt = new ProgressBarFormat(full: '=', tip: '>', left: "[", right: "]", paddingLeft: 1, hiddenWhen: () => pbar.Complete);
            pbar = new ProgressBar(10, format: fmt);

            Loop(pbar, null, null, (pb, ps, _) => Console.Write($"\rThe progress bar will disappear when complete...{pb} Some other text.".PadRight(Console.WindowWidth - 1)));
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
            // "⠋⠙⠹⠸⠼⠴⠦⠧⠇⠏"
            var progressBar = new ProgressBar(50, 0, 100, 1, 0);
            Loop(progressBar, new Spinner(new SpinnerFormat(complete: '√', completeWhen: () => progressBar.Complete)), new Marquee("abcd", 20, new MarqueeFormat(complete: "Done!", completeWhen: () => progressBar.Complete, leftToRight: false, bounce: true, reverseTextOnBounce: true)), (pb, ps, m) =>
            {
                Console.Write($"\r{pb} {m} {ps}".PadRight(Console.WindowWidth - 1));
            });
            //Loop(new ProgressBar(10, 0, 1000, 1, 0), null, new Marquee("Hello, World!", 30, new MarqueeFormat(leftToRight: false, bounce: true, reverseTextOnBounce: false, gap:0)), (pb, ps, m) =>
            //{
            //    Console.Write($"\r{pb} {m}");
            //    m.PerformStep();
            //});
        }
    }
}
