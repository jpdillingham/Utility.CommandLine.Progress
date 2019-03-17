using System;
using System.Text;

namespace Utility.CommandLine.ProgressBar
{
    public class ProgressBar
    {
        private int _value;

        public int Minimum { get; }
        public int Maximum { get; }
        public int Step { get; }
        public ProgressBarFormat Format { get; }
        public double Percent => Value / (double)Maximum;
        public int Width { get; }

        public int Value {
            get
            {
                return _value;
            }

            set
            {
                if (value > Maximum || value < Minimum)
                {
                    throw new ArgumentOutOfRangeException(nameof(Value), value, $"Value must be between {Minimum} and {Maximum}, inclusive.");
                }

                _value = value;
            }
        }

        public ProgressBar(int width = 0, int minimum = 0, int maximum = 100, int step = 1, int value = 0, ProgressBarFormat format = null)
        {
            Width = width;
            Minimum = minimum;
            Maximum = maximum;
            Step = step;
            Value = value;
            Format = format ?? new ProgressBarFormat();
        }

        public void PerformStep()
        {
            Value += Step;
        }

        public override string ToString()
        {
            var percentFull = Value / (double)Maximum;
            var width = Width < 1 ? Console.WindowWidth - Math.Abs(Width) - 4 : Width;

            var chars = (int)Math.Round(width * percentFull, 0);

            var builder = new StringBuilder();
            builder.Append(Format.Start);
            builder.Append(new string(Format.Full, chars));
            builder.Append(chars > 0 ? Format.Tip.ToString() : string.Empty);
            builder.Append(new string(Format.Empty, width - chars));
            builder.Append(Format.End);

            return builder.ToString();
        }
    }

    public class ProgressBarFormat
    {
        public ProgressBarFormat(char empty = ' ', char full = '█', char tip = '█', char start = '[', char end = ']')
        {
            Empty = empty;
            Full = full;
            Tip = tip;
            Start = start;
            End = end;
        }

        public char Start { get; }
        public char End { get; }
        public char Empty { get; }
        public char Full { get; }
        public char Tip { get; }
    }
}
