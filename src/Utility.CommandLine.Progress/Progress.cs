using System;
using System.Linq;
using System.Text;

namespace Utility.CommandLine.ProgressBar
{
    public class Spinner
    {
        public int Frame { get; private set; }
        public char[] Frames { get; }
        public SpinnerFormat Format { get; }

        public Spinner(SpinnerFormat format = null)
            : this(new[] { '-', '\\', '|', '/' }, format)
        {
        }

        public Spinner(params char[] frames)
            : this(frames, null)
        {
        }

        public Spinner(char[] frames, SpinnerFormat format = null)
        {
            Frame = 0;
            Frames = frames;
            Format = format ?? new SpinnerFormat();
        }

        public void Spin()
        {
            Frame = ++Frame % Frames.Length;
        }

        public override string ToString()
        {
            if (Format.HiddenWhen())
            {
                return string.Empty;
            }

            if (Format.EmptyWhen())
            {
                return new string(' ', Format.PaddingLeft + (Format.Left?.Length ?? 0) + 1 + (Format.Right?.Length ?? 0) + Format.PaddingRight);
            }

            var builder = new StringBuilder();
            builder.Append(new string(Format.Pad, Format.PaddingLeft));
            builder.Append(Format.Left);

            builder.Append(Frames[Frame]);

            builder.Append(Format.Right);
            builder.Append(new string(Format.Pad, Format.PaddingRight));

            return builder.ToString();
        }
    }

    public class SpinnerFormat
    {
        public SpinnerFormat(char empty = ' ', string left = null, string right = null, int paddingLeft = 0, int paddingRight = 0, char pad = ' ', Func<bool> emptyWhen = null, Func<bool> hiddenWhen = null)
        {
            Left = left;
            Right = right;
            Pad = pad;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;

            EmptyWhen = emptyWhen ?? (() => false);
            HiddenWhen = hiddenWhen ?? (() => false);
        }

        public string Left { get; }
        public string Right { get; }
        public char Pad { get; }
        public int PaddingRight { get; }
        public int PaddingLeft { get; }
        public Func<bool> EmptyWhen { get; }
        public Func<bool> HiddenWhen { get; }
    }

    public class Marquee
    {
        public string Text { get; private set; }
        public int Width { get; }
        public MarqueeFormat Format { get; }
        public int Position { get; private set; }
        public bool Reversed { get; private set; }
        public int Gap { get; }

        private string _text;

        public Marquee(string text, int width = 0, MarqueeFormat format = null)
        {
            Text = text;
            Width = width;
            Format = format ?? new MarqueeFormat();
            Position = Text.Length + Width;
            Gap = Format.Gap ?? Width;

            SetText();
        }

        private void SetText()
        {
            _text = string.Empty;

            do
            {
                _text += new string(Format.Empty, Gap) + Text;
            } while (_text.Length < Width);
        }

        public void Scroll()
        {
            if (Format.LeftToRight ^ Reversed)
            {
                ScrollRight();
            }
            else
            {
                ScrollLeft();
            }

            if (Position == 0 && Format.Bounce)
            {
                Reversed = !Reversed;

                if (Format.ReverseTextOnBounce)
                {
                    Text = new string(Text.Reverse().ToArray());
                    SetText();
                }
            }
        }

        public override string ToString()
        {
            if (Format.HiddenWhen())
            {
                return string.Empty;
            }

            if (Format.EmptyWhen())
            {
                return new string(Format.Empty, Format.PaddingLeft + (Format.Left?.Length ?? 0) + Width + (Format.Right?.Length ?? 0) + Format.PaddingRight);
            }

            var width = Width < 1 ? Console.WindowWidth - Math.Abs(Width) + 1: Width;

            var builder = new StringBuilder();
            builder.Append(new string(Format.Pad, Format.PaddingLeft));
            builder.Append(Format.Left);

            builder.Append(new string(_text.Take(width).ToArray()));
 
            builder.Append(Format.Right);
            builder.Append(new string(Format.Pad, Format.PaddingRight));

            return builder.ToString();
        }

        private void ScrollLeft()
        {
            _text = new string(_text.Skip(1).ToArray()) + new string(_text.Take(1).ToArray());
            Position = --Position % (Text.Length + Width);
        }

        private void ScrollRight()
        {
            _text = new string(_text.Skip(_text.Length - 1).ToArray()) + new string(_text.Take(_text.Length - 1).ToArray());
            Position = ++Position % (Text.Length + Width);
        }
    }

    public class MarqueeFormat
    {
        public MarqueeFormat(char empty = ' ', string left = null, string right = null, int paddingLeft = 0, int paddingRight = 0, char pad = ' ', bool bounce = false, bool reverseTextOnBounce = false, bool leftToRight = false, int? gap = null, Func<bool> emptyWhen = null, Func<bool> hiddenWhen = null)
        {
            Empty = empty;
            Left = left;
            Right = right;
            Pad = pad;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;
            Bounce = bounce;
            ReverseTextOnBounce = reverseTextOnBounce;
            LeftToRight = leftToRight;
            Gap = gap;

            EmptyWhen = emptyWhen ?? (() => false);
            HiddenWhen = hiddenWhen ?? (() => false);
        }

        public Func<bool> EmptyWhen { get; }
        public Func<bool> HiddenWhen { get; }

        public char Empty { get; }
        public string Left { get; }
        public string Right { get; }
        public bool Bounce { get; }
        public char Pad { get; }
        public bool ReverseTextOnBounce { get; }
        public int PaddingRight { get; }
        public int PaddingLeft { get; }
        public bool LeftToRight { get; }
        public int? Gap { get; }
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

        public void Increment(int steps = 1)
        {
            Value += Step * steps;
        }

        public void Decrement(int steps = 1)
        {
            Value -= Step * steps;
        }

        public override string ToString()
        {
            if (Format.HiddenWhen())
            {
                return string.Empty;
            }

            var width = Width < 1 ? Console.WindowWidth - Math.Abs(Width) - 4 : Width;

            if (Format.EmptyWhen())
            {
                return new string(' ', Format.PaddingLeft + (Format.Left?.Length ?? 0) + width + (Format.Right?.Length ?? 0) + Format.PaddingRight);
            }


            var percentFull = Value / (double)Maximum;

            var chars = (int)Math.Round(width * percentFull, 0);

            var builder = new StringBuilder();
            builder.Append(new string(Format.Pad, Format.PaddingLeft));
            builder.Append(Format.Left);
            builder.Append(new string(Format.Full, chars));
            builder.Append(chars > 0 ? chars == width ? Format.Full : Format.Tip : Format.Empty);
            builder.Append(new string(Format.Empty, width - chars));
            builder.Append(Format.Right);
            builder.Append(new string(' ', Format.PaddingRight));

            return builder.ToString();
        }
    }

    public class ProgressBarFormat
    {
        public ProgressBarFormat(char empty = '░', char full = '█', char tip = '█', string left = null, string right = null, int paddingLeft = 0, int paddingRight = 0, char pad = ' ', Func<bool> emptyWhen = null, Func<bool> hiddenWhen = null)
        {
            Empty = empty;
            Full = full;
            Tip = tip;
            Left = left;
            Right = right;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;
            Pad = pad;

            EmptyWhen = emptyWhen ?? (() => false);
            HiddenWhen = hiddenWhen ?? (() => false);
        }

        public char Empty { get; }
        public char Full { get; }
        public char Tip { get; }
        public char Pad { get; }
        public string Left { get; }
        public string Right { get; }
        public int PaddingRight { get; }
        public int PaddingLeft { get; }
        public Func<bool> EmptyWhen { get; }
        public Func<bool> HiddenWhen { get; }
    }

    public abstract class ProgressFormat
    {
        public ProgressFormat(char empty = ' ', string left = null, string right = null, int paddingLeft = 0, int paddingRight = 0, char pad = ' ', Func<bool> emptyWhen = null, Func<bool> hiddenWhen = null)
        {
            Empty = empty;
            Left = left;
            Right = right;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;
            Pad = pad;

            EmptyWhen = emptyWhen ?? (() => false);
            HiddenWhen = hiddenWhen ?? (() => false);
        }

        public char Empty { get; }
        public char Pad { get; }
        public string Left { get; }
        public string Right { get; }
        public int PaddingRight { get; }
        public int PaddingLeft { get; }
        public Func<bool> EmptyWhen { get; }
        public Func<bool> HiddenWhen { get; }
    }
}
