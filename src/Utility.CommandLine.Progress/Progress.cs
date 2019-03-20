using System;
using System.Linq;
using System.Text;

namespace Utility.CommandLine.ProgressBar
{
    public class Spinner
    {
        public int CurrentFrame { get; private set; }
        public char[] Frames { get; }

        public Spinner()
            : this('-', '\\', '|', '/')
        {
        }

        public void PerformStep()
        {
            CurrentFrame = ++CurrentFrame % Frames.Length;
        }

        public Spinner(params char[] frames)
        {
            CurrentFrame = 0;
            Frames = frames;
        }

        public override string ToString()
        {
            return Frames[CurrentFrame].ToString();
        }
    }

    public class Marquee
    {
        public string Text { get; private set; }
        public int Width { get; }
        public MarqueeFormat Format { get; }
        public int CurrentPosition { get; private set; }
        public bool Reversed { get; private set; }

        private string _text;

        public Marquee(string text, int width = 0, MarqueeFormat format = null)
        {
            Text = text;
            Width = width;
            Format = format ?? new MarqueeFormat();
            CurrentPosition = Text.Length + Width;

            _text = new string(Format.Empty, Width) + Text;
        }

        public void PerformStep()
        {
            if (Format.LeftToRight ^ Reversed)
            {
                RotateRight();
            }
            else
            {
                RotateLeft();
            }

            if (CurrentPosition == 0 && Format.Bounce)
            {
                Reversed = !Reversed;
                Text = new string(Text.Reverse().ToArray());

                _text = new string(Format.Empty, Width) + Text;
            }
        }

        public override string ToString()
        {
            var width = Width < 1 ? Console.WindowWidth - Math.Abs(Width) - 4 : Width;

            var builder = new StringBuilder();
            builder.Append(new string(Format.Pad, Format.PaddingLeft));
            builder.Append(Format.Start);

            builder.Append(new string(_text.Take(width).ToArray()));
 
            builder.Append(Format.End);
            builder.Append(new string(Format.Pad, Format.PaddingRight));

            return builder.ToString();
        }

        private void RotateLeft()
        {
            _text = new string(_text.Skip(1).ToArray()) + new string(_text.Take(1).ToArray());
            CurrentPosition = --CurrentPosition % (Text.Length + Width);
        }

        private void RotateRight()
        {
            _text = new string(_text.Skip(_text.Length - 1).ToArray()) + new string(_text.Take(_text.Length - 1).ToArray());
            CurrentPosition = ++CurrentPosition % (Text.Length + Width);
        }
    }

    public class MarqueeFormat
    {
        public MarqueeFormat(char empty = ' ', char? start = null, char? end = null, int paddingLeft = 0, int paddingRight = 0, char pad = ' ', bool bounce = false, bool reverseTextOnBounce = false, bool leftToRight = false)
        {
            Empty = empty;
            Start = start;
            End = end;
            Pad = pad;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;
            Bounce = bounce;
            ReverseTextOnBounce = reverseTextOnBounce;
            LeftToRight = leftToRight;
        }

        public char Empty { get; }
        public char? Start { get; }
        public char? End { get; }
        public bool Bounce { get; }
        public char Pad { get; }
        public bool ReverseTextOnBounce { get; }
        public int PaddingRight { get; }
        public int PaddingLeft { get; }
        public bool LeftToRight { get; }
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
        public ProgressBarFormat(char empty = '░', char full = '█', char tip = '█', char? start = null, char? end = null, bool hideWhenComplete = false, int paddingLeft = 0, int paddingRight = 0, char pad = ' ')
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
        public char? Start { get; }
        public char? End { get; }
        public int PaddingRight { get; }
        public int PaddingLeft { get; }
    }
}
