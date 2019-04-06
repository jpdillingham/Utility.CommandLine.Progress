/*
  █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀  ▀  ▀      ▀▀
  █  The MIT License (MIT)
  █
  █  Copyright (c) 2019 JP Dillingham (jp@dillingham.ws)
  █
  █  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
  █  files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
  █  modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
  █  Software is furnished to do so, subject to the following conditions:
  █
  █  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
  █
  █  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  █  WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
  █  COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
  █  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
  █
  ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀  ▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀██
                                                                                               ██
                                                                                           ▀█▄ ██ ▄█▀
                                                                                             ▀████▀
                                                                                               ▀▀                                */

namespace Utility.CommandLine.Progress.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AutoFixture.Xunit2;
    using Xunit;

    public class SpinnerTests
    {
        [Fact(DisplayName = "Spinner instantiates with defaults and given values")]
        public void Spinner_Instantiates_With_Defaults_And_Given_Values()
        {
            var s1 = new Spinner();
            var s2 = new Spinner('+', '-');

            var fmt = new SpinnerFormat();
            var s3 = new Spinner(new[] { 'a', 'b', 'c' }, format: fmt);

            var s4 = new Spinner("xyz", false, fmt);

            var s5 = new Spinner(null);

            Assert.Equal(new[] { '-', '\\', '|', '/' }, s1.Frames);
            Assert.Equal(0, s1.Frame);

            Assert.Equal(new[] { '+', '-' }, s2.Frames);

            Assert.Equal(new[] { 'a', 'b', 'c' }, s3.Frames);
            Assert.Equal(fmt, s3.Format);

            Assert.Equal(new[] { 'x', 'y', 'z' }, s4.Frames);
            Assert.False(s4.SpinOnToString);
            Assert.Equal(fmt, s4.Format);

            Assert.Equal(new[] { '-', '\\', '|', '/' }, s5.Frames);
        }

        [Fact(DisplayName = "Spinner respects CompleteWhen")]
        public void Spinner_Respects_CompleteWhen()
        {
            var complete = false;
            var s = new Spinner(format: new SpinnerFormat(complete: 'a', completeWhen: () => complete));

            var c1 = s.ToString();

            complete = true;
            var c2 = s.ToString();

            Assert.NotEqual("a", c1);
            Assert.Equal("a", c2);
        }

        [Fact(DisplayName = "Spinner respects EmptyWhen")]
        public void Spinner_Respects_EmptyWhen()
        {
            var empty = false;
            var s = new Spinner(format: new SpinnerFormat(emptyWhen: () => empty));

            var empty1 = s.ToString();

            empty = true;
            var empty2 = s.ToString();

            Assert.NotEqual(s.Format.Empty.ToString(CultureInfo.InvariantCulture), empty1);
            Assert.Equal(s.Format.Empty.ToString(CultureInfo.InvariantCulture), empty2);
        }

        [Fact(DisplayName = "Spinner respects HiddenWhen")]
        public void Spinner_Respects_HiddenWhen()
        {
            var hidden = false;
            var s = new Spinner(format: new SpinnerFormat(hiddenWhen: () => hidden));

            var hidden1 = s.ToString();

            hidden = true;
            var hidden2 = s.ToString();

            Assert.NotEqual(string.Empty, hidden1);
            Assert.Equal(string.Empty, hidden2);
        }

        [Fact(DisplayName = "Spinner returns formatted output")]
        public void Spinner_Returns_Formatted_Output()
        {
            var s = new Spinner(spinOnToString: false, format: new SpinnerFormat(left: "[", right: "]", paddingLeft: 1, paddingRight: 1, pad: '.'));

            Assert.Equal(".[-].", s.ToString());
        }

        [Fact(DisplayName = "Spinner.Spin advances all frames in order")]
        public void Spinner_Spin_Advances_All_Frames_In_Order()
        {
            var frames = new List<string>();

            var s = new Spinner("abc", spinOnToString: false);

            for (int i = 0; i < 5; i++)
            {
                frames.Add(s.ToString());
                s.Spin();
            }

            Assert.Equal("a", frames[0]);
            Assert.Equal("b", frames[1]);
            Assert.Equal("c", frames[2]);
            Assert.Equal("a", frames[3]);
        }

        [Fact(DisplayName = "Spinner.Spin advances frame")]
        public void Spinner_Spin_Advances_Frame()
        {
            var s = new Spinner(spinOnToString: false);

            var frames = s.Frames;
            var initialString = s.ToString();
            var initialFrame = s.Frame;

            s.Spin();

            Assert.Equal(0, initialFrame);
            Assert.Equal(frames.ToArray()[initialFrame].ToString(CultureInfo.InvariantCulture), initialString);

            Assert.Equal(1, s.Frame);
            Assert.Equal(frames.ToArray()[initialFrame + 1].ToString(CultureInfo.InvariantCulture), s.ToString());
        }

        [Fact(DisplayName = "Spinner.ToString() advances frame when SpinOnToString")]
        public void Spinner_ToString_Advances_Frame_When_SpinOnToString()
        {
            var s = new Spinner(spinOnToString: true);

            var frame1 = s.Frame;
            var _ = s.ToString();
            var frame2 = s.Frame;

            Assert.Equal(frame2, frame1 + 1);
        }

        [Theory(DisplayName = "SpinnerFormat instantiates with the given values"), AutoData]
        public void SpinnerFormat_Instantiates_With_The_Given_Values(char empty, char complete, string left, string right, int paddingLeft, int paddingRight, char pad)
        {
#pragma warning disable IDE0039 // Use local function
            Func<bool> c = () => true;
            Func<bool> e = () => true;
            Func<bool> h = () => false;
#pragma warning restore IDE0039 // Use local function

            SpinnerFormat f = null;
            var ex = Record.Exception(() => f = new SpinnerFormat(empty, complete, left, right, paddingLeft, paddingRight, pad, c, e, h));

            Assert.Null(ex);

            Assert.Equal(empty, f.Empty);
            Assert.Equal(complete, f.Complete);
            Assert.Equal(left, f.Left);
            Assert.Equal(right, f.Right);
            Assert.Equal(paddingLeft, f.PaddingLeft);
            Assert.Equal(paddingRight, f.PaddingRight);
            Assert.Equal(pad, f.Pad);
            Assert.Equal(paddingLeft + paddingRight + left.Length + right.Length, f.Width);

            Assert.Equal(c, f.CompleteWhen);
            Assert.Equal(e, f.EmptyWhen);
            Assert.Equal(h, f.HiddenWhen);
        }
    }
}