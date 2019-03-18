using System;
using System.Text;

namespace Utility.CommandLine.ProgressBar
{
    public class ProgressSpinner
    {
        public int CurrentFrame { get; private set; }
        public char[] Frames { get; }

        public ProgressSpinner()
            : this('-', '\\', '|', '/')
        {
        }

        public ProgressSpinner(params char[] frames)
        {
            CurrentFrame = 0;
            Frames = frames;
        }

        public override string ToString()
        {
            CurrentFrame = ++CurrentFrame % Frames.Length;
            return Frames[CurrentFrame].ToString();
        }
    }

    public class Marquee
    {
        public string Text { get; }
        public int Width { get; }
        public MarqueeFormat Format { get; }

        public Marquee(string text = "", MarqueeFormat format = null)
        {
            Text = text;
            Format = format ?? new MarqueeFormat();
        }
    }

    public class MarqueeFormat
    {
        public MarqueeFormat(char empty = ' ', char start = '[', char end = ']', bool bounce = false, bool reverseOnBounce = false)
        {
            Empty = empty;
            Start = start;
            End = end;
            Bounce = bounce;
            ReverseOnBounce = reverseOnBounce;
        }

        public char Empty { get; }
        public char Start { get; }
        public char End { get; }
        public bool Bounce { get; }
        public bool ReverseOnBounce { get; }
    }

    public class ProgressBar
    {
        private int _value;

        public int Minimum { get; }
        public int Maximum { get; }
        public int Step { get; }
        public ProgressBarFormat Format { get; }
        public double Percent => Value / (double)Maximum;
        public bool Complete => Value == Maximum;
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
            if (Complete && Format.HideWhenComplete)
            {
                return string.Empty;
            }

            var percentFull = Value / (double)Maximum;
            var width = Width < 1 ? Console.WindowWidth - Math.Abs(Width) - 4 : Width;

            var chars = (int)Math.Round(width * percentFull, 0);

            var builder = new StringBuilder();
            builder.Append(new string(Format.Pad, Format.PaddingLeft));
            builder.Append(Format.Start);
            builder.Append(new string(Format.Full, chars));
            builder.Append(chars > 0 ? chars == width ? Format.Full : Format.Tip : Format.Empty);
            builder.Append(new string(Format.Empty, width - chars));
            builder.Append(Format.End);
            builder.Append(new string(' ', Format.PaddingRight));

            return builder.ToString();
        }
    }

    public class ProgressBarFormat
    {
        public ProgressBarFormat(char empty = ' ', char full = '█', char tip = '█', char start = '[', char end = ']', bool hideWhenComplete = false, int paddingLeft = 0, int paddingRight = 0, char pad = ' ')
        {
            Empty = empty;
            Full = full;
            Tip = tip;
            Start = start;
            End = end;
            HideWhenComplete = hideWhenComplete;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;
            Pad = pad;
        }

        public bool HideWhenComplete { get; }
        public char Empty { get; }
        public char Full { get; }
        public char Tip { get; }
        public char Pad { get; }
        public char Start { get; }
        public char End { get; }
        public int PaddingRight { get; }
        public int PaddingLeft { get; }
    }
}
