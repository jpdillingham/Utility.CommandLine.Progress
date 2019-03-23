/*
  █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀  ▀  ▀      ▀▀
  █  The MIT License (MIT)
  █
  █  Copyright (c) 2019 JP Dillingham (jp@dillingham.ws)
  █
  █  Permission is hereby granted, free of charge, to any person obtaining a copy
  █  of this software and associated documentation files (the "Software"), to deal
  █  in the Software without restriction, including without limitation the rights
  █  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  █  copies of the Software, and to permit persons to whom the Software is
  █  furnished to do so, subject to the following conditions:
  █
  █  The above copyright notice and this permission notice shall be included in all
  █  copies or substantial portions of the Software.
  █
  █  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  █  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  █  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  █  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  █  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  █  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  █  SOFTWARE.
  █
  ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀  ▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀██
                                                                                               ██
                                                                                           ▀█▄ ██ ▄█▀
                                                                                             ▀████▀
                                                                                               ▀▀                            */

namespace Utility.CommandLine.ProgressBar.Tests
{
    using AutoFixture.Xunit2;
    using System;
    using System.Linq;
    using Utility.CommandLine.Progress;
    using Xunit;

    public class ProgressTests
    {
        [Fact(DisplayName = "Meta test")]
        public void Meta_Test()
        {
            Assert.True(true);
        }

        [Theory(DisplayName = "SpinnerFormat instantiates with the given values"), AutoData]
        public void SpinnerFormat_Instantiates_With_The_Given_Values(char empty, string left, string right, int paddingLeft, int paddingRight, char pad)
        {
            Func<bool> e = () => true;
            Func<bool> h = () => false;

            SpinnerFormat f = null;
            var ex = Record.Exception(() => f = new SpinnerFormat(empty, left, right, paddingLeft, paddingRight, pad, e, h));

            Assert.Null(ex);

            Assert.Equal(empty, f.Empty);
            Assert.Equal(left, f.Left);
            Assert.Equal(right, f.Right);
            Assert.Equal(paddingLeft, f.PaddingLeft);
            Assert.Equal(paddingRight, f.PaddingRight);
            Assert.Equal(pad, f.Pad);

            Assert.Equal(e, f.EmptyWhen);
            Assert.Equal(h, f.HiddenWhen);
        }

        [Fact(DisplayName = "Spinner instantiates with defaults and given values")]
        public void Spinner_Instantiates_With_Defaults_And_Given_Values()
        {
            var s1 = new Spinner();
            var s2 = new Spinner('+', '-');

            var fmt = new SpinnerFormat();
            var s3 = new Spinner(new[] { 'a', 'b', 'c' }, fmt);

            Assert.Equal(new[] { '-', '\\', '|', '/' }, s1.Frames);
            Assert.Equal(0, s1.Frame);

            Assert.Equal(new[] { '+', '-' }, s2.Frames);

            Assert.Equal(new[] { 'a', 'b', 'c' }, s3.Frames);
            Assert.Equal(fmt, s3.Format);
        }

        [Fact(DisplayName = "Spinner.Spin advances frame")]
        public void Spinner_Spin_Advances_Frame()
        {
            var s = new Spinner();

            var frames = s.Frames;
            var initialString = s.ToString();
            var initialFrame = s.Frame;

            s.Spin();

            Assert.Equal(0, initialFrame);
            Assert.Equal(frames.ToArray()[initialFrame].ToString(), initialString);

            Assert.Equal(1, s.Frame);
            Assert.Equal(frames.ToArray()[initialFrame + 1].ToString(), s.ToString());
        }
    }
}
