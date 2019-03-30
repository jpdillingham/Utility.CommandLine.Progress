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

namespace Utility.CommandLine.Progress.Tests
{
    using AutoFixture.Xunit2;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utility.CommandLine.Progress;
    using Xunit;

    public class MarqueeTests
    {
        [Theory(DisplayName = "MarqueeFormat instantiates with the given values"), AutoData]
        public void MarqueeFormat_Instantiates_With_The_Given_Values(char empty, string complete, string left, string right, int paddingLeft, int paddingRight, char pad, bool bounce, bool reverseTextOnBounce, bool leftToRight, int? gap)
        {
#pragma warning disable IDE0039 // Use local function
            Func<bool> c = () => true;
            Func<bool> e = () => true;
            Func<bool> h = () => false;
#pragma warning restore IDE0039 // Use local function

            MarqueeFormat f = null;
            var ex = Record.Exception(() => f = new MarqueeFormat(empty, complete, left, right, paddingLeft, paddingRight, pad, bounce, reverseTextOnBounce, leftToRight, gap, c, e, h));

            Assert.Null(ex);

            Assert.Equal(empty, f.Empty);
            Assert.Equal(complete, f.Complete);
            Assert.Equal(left, f.Left);
            Assert.Equal(right, f.Right);
            Assert.Equal(paddingLeft, f.PaddingLeft);
            Assert.Equal(paddingRight, f.PaddingRight);
            Assert.Equal(pad, f.Pad);
            Assert.Equal(bounce, f.Bounce);
            Assert.Equal(reverseTextOnBounce, f.ReverseTextOnBounce);
            Assert.Equal(leftToRight, f.LeftToRight);
            Assert.Equal(gap, f.Gap);
            Assert.Equal(paddingLeft + paddingRight + left.Length + right.Length, f.Width);

            Assert.Equal(c, f.CompleteWhen);
            Assert.Equal(e, f.EmptyWhen);
            Assert.Equal(h, f.HiddenWhen);
        }

        [Theory(DisplayName = "Marquee instantiates with the given values"), AutoData]
        public void Marquee_Instantiates_With_The_Given_Values(string text, int width, MarqueeFormat format)
        {
            Marquee m = null;
            var ex = Record.Exception(() => m = new Marquee(text, width, format));

            Assert.Null(ex);

            Assert.Equal(text, m.Text);
            Assert.Equal(width, m.Width);
            Assert.Equal(format, m.Format);

            Assert.Equal(format.Gap, m.Gap);
            Assert.Equal(text.Length + width, m.Position);
            Assert.False(m.Reversed);
        }
    }
}
