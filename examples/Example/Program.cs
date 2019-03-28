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
                new Spinner("-\\|/", format: new SpinnerFormat(completeWhen: () => fullWidth.Complete)),
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

                Thread.Sleep(100);
            }
        }
    }
}
