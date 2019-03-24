using System;
using System.Collections.Generic;
using System.Threading;
using Utility.CommandLine.Progress;

namespace Example
{
    class Program
    {
        static List<Action> lines = new List<Action>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            var fullWidth = new ProgressBar();
            lines.Add(() =>
            {
                fullWidth.Increment();
                Console.Write(fullWidth);
            });

            var fixedWidth = new ProgressBar(20, format: new ProgressBarFormat(left: "[", right: "]", full: '=', tip: '>', empty: ' '));
            lines.Add(() =>
            {
                fixedWidth.Increment();
                Console.Write($"Here's a fixed width example: {fixedWidth}");
            });

            var fixedWidthEmpty = new ProgressBar(20, format: new ProgressBarFormat(left: "[", right: "]", full: '=', tip: '>', empty: ' ', emptyWhen: () => fixedWidth.Complete));
            lines.Add(() =>
            {
                fixedWidthEmpty.Increment();
                Console.Write($"Here's one that appears empty when complete: {fixedWidthEmpty}!");
            });

            var fixedWidthHidden = new ProgressBar(20, format: new ProgressBarFormat(left: "[", right: "]", full: '=', tip: '>', empty: ' ', hiddenWhen: () => fixedWidth.Complete));
            lines.Add(() =>
            {
                fixedWidthHidden.Increment();
                Console.Write($"Here's one that is hidden when complete: {fixedWidthHidden}!".PadRight(Console.WindowWidth - 1));
            });

            var fitToWidth = new ProgressBar(format: new ProgressBarFormat(left: "Here's one that fits to width: "));
            lines.Add(() =>
            {
                fitToWidth.Increment();
                Console.Write(fitToWidth);
            });

            var withPercentage = new ProgressBar(width: -8, format: new ProgressBarFormat(left: "Here's one that shows percentage: "));
            lines.Add(() =>
            {
                withPercentage.Increment();
                Console.Write($"{withPercentage} [{(withPercentage.Percent * 100).ToString().PadLeft(3)}%]".PadRight(Console.WindowWidth - 1));
            });

            var helloWorld = new Marquee("Hello, World!", 15);
            lines.Add(() =>
            {
                helloWorld.Scroll();
                Console.Write($"Here's a marquee: {helloWorld}");
            });

            var helloWorld2 = new Marquee("Hello, World!", 15, new MarqueeFormat(leftToRight: true));
            lines.Add(() =>
            {
                helloWorld2.Scroll();
                Console.Write($"Here's one that goes the other way: {helloWorld2}");
            });

            var helloWorld3 = new Marquee("Hello, World!", 15, new MarqueeFormat(bounce: true));
            lines.Add(() =>
            {
                helloWorld3.Scroll();
                Console.Write($"Here's one that bounces: {helloWorld3}");
            });

            var knightRider = new Marquee("█▓▒░", 15, new MarqueeFormat(bounce: true, reverseTextOnBounce: true));
            lines.Add(() =>
            {
                knightRider.Scroll();
                Console.Write($"Here's one like Knight Rider: {knightRider}");
            });

            var slashes = new Marquee("/", 15, new MarqueeFormat(gap: 1));
            lines.Add(() =>
            {
                slashes.Scroll();
                Console.Write($"Here's one with a small gap: {slashes}");
            });

            var doingDone = new Marquee("Doing a thing...", 15, new MarqueeFormat(gap: 1, left: "[", right: "]", complete: "Done!", completeWhen: () => fullWidth.Complete));
            lines.Add(() =>
            {
                doingDone.Scroll();
                Console.Write($"Here's one that changes when complete: {doingDone}");
            });

            var spinners = new List<Spinner>()
            {
                new Spinner("-\\|/", new SpinnerFormat(completeWhen: () => fullWidth.Complete)),
                new Spinner("⠋⠙⠹⠸⠼⠴⠦⠧⠇⠏"),
                new Spinner("⢄⢂⢁⡁⡈⡐⡠"),
                new Spinner("▁▃▄▅▆▇▆▅▄▃"),
                new Spinner("▏▎▍▌▋▊▉▊▋▌▍▎"),
                new Spinner("⠁⠂⠄⠂"),
                new Spinner("⠂-–—–-")
            };
            lines.Add(() =>
            {
                spinners.ForEach(s => s.Spin());
                Console.Write($"Here's some spinners: {string.Join("   ", spinners)}");
            });

            for (int i = 0; i < lines.Count; i++) { Console.WriteLine(); }

            for (int i = 0; i < 100; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - lines.Count);

                foreach (var line in lines)
                {
                    line();
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }

                Thread.Sleep(75);
            }

            //FullWidth();
            //FitToWidth();
            //FixedWidth();
            //Disappear();
            //WithSpinner();
            //BlockySpinner();
            //BlockySpinner2();
            //Marquee();
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
            var frames1 = "⠋⠙⠹⠸⠼⠴⠦⠧⠇⠏";
            var frames2 = "⢄⢂⢁⡁⡈⡐⡠";
            var frames3 = "▁▃▄▅▆▇▆▅▄▃";
            var frames4 = "▏▎▍▌▋▊▉▊▋▌▍▎";
            var frames5 = "⠁⠂⠄⠂";

            var frames = new char[]{ '⠂', '-', '–', '—', '–', '-' };
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var progressBar = new ProgressBar(50, 0, 100, 1, 0);
            Loop(progressBar, new Spinner(frames1, new SpinnerFormat(complete: '√', completeWhen: () => progressBar.Complete)), new Marquee("abcd", 20, new MarqueeFormat(complete: "Done!", completeWhen: () => progressBar.Complete, leftToRight: false, bounce: true, reverseTextOnBounce: true)), (pb, ps, m) =>
            {
                Console.Write($"\r{pb} {m} {ps}".PadRight(Console.WindowWidth));
            });
            //Loop(new ProgressBar(10, 0, 1000, 1, 0), null, new Marquee("Hello, World!", 30, new MarqueeFormat(leftToRight: false, bounce: true, reverseTextOnBounce: false, gap:0)), (pb, ps, m) =>
            //{
            //    Console.Write($"\r{pb} {m}");
            //    m.PerformStep();
            //});
        }
    }
}
