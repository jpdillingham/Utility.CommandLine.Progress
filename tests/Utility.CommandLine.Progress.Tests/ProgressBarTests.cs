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

    public class ProgressBarTests
    {
        [Theory(DisplayName = "ProgressBarFormat instantiates with the given values"), AutoData]
        public void ProgressBarFormat_Instantiates_With_The_Given_Values(char empty, char full, char tip, string left, string right, int paddingLeft, int paddingRight, char pad)
        {
#pragma warning disable IDE0039 // Use local function
            Func<bool> e = () => true;
            Func<bool> h = () => false;
#pragma warning restore IDE0039 // Use local function

            ProgressBarFormat f = null;
            var ex = Record.Exception(() => f = new ProgressBarFormat(empty, full, tip, left, right, paddingLeft, paddingRight, pad, e, h));

            Assert.Equal(empty, f.Empty);
            Assert.Equal(full, f.Full);
            Assert.Equal(tip, f.Tip);
            Assert.Equal(left, f.Left);
            Assert.Equal(right, f.Right);
            Assert.Equal(paddingLeft, f.PaddingLeft);
            Assert.Equal(paddingRight, f.PaddingRight);
            Assert.Equal(pad, f.Pad);
            Assert.Equal(e, f.EmptyWhen);
            Assert.Equal(h, f.HiddenWhen);
        }
        
        [Theory(DisplayName = "ProgressBar instantiates with the given values"), AutoData]
        public void ProgressBar_Instantiates_With_The_Given_Values(int width, int minimum, int maximum, int step, ProgressBarFormat format)
        {
            if (width == 0)
            {
                width++;
            }

            maximum = Math.Abs(maximum);
            minimum = Math.Abs(minimum);

            if (minimum > maximum)
            {
                var t = maximum;
                maximum = minimum;
                minimum = maximum;
            }

            if (minimum == maximum)
            {
                maximum++;
            }

            var value = new Random().Next(minimum, maximum);
            var p = new ProgressBar(width, minimum, maximum, step, value, format);

            Assert.Equal(width, p.Width);
            Assert.Equal(minimum, p.Minimum);
            Assert.Equal(maximum, p.Maximum);
            Assert.Equal(step, p.Step);
            Assert.Equal(value, p.Value);
            Assert.Equal(format, p.Format);

            Assert.Equal(p.Value == p.Maximum, p.Complete);
            Assert.Equal(p.Value / (double)p.Maximum, p.Percent);
        }

        [Theory(DisplayName = "ProgresBar clamps value")]
        [InlineData(101, 0, 100, 100)]
        [InlineData(-1, 0, 100, 0)]
        [InlineData(50, 0, 100, 50)]
        public void ProgressBar_Clamps_Value(int num, int min, int max, int expected)
        {
            var p = new ProgressBar(10, min, max, 1, 0);

            p.Value = num;

            Assert.Equal(expected, p.Value);
        }

        [Theory(DisplayName = "ProgressBar increments by step"), AutoData]
        public void ProgressBar_Increments_By_Step(int step)
        {
            step = Math.Abs(step);
            var p = new ProgressBar(10, step: step, maximum: step * 2);

            var v1 = p.Value;
            p.Increment();
            var v2 = p.Value;
            p.Increment();
            var v3 = p.Value;

            Assert.Equal(0, v1);
            Assert.Equal(step, v2);
            Assert.Equal(step * 2, v3);
        }

        [Theory(DisplayName = "ProgressBar decrements by step"), AutoData]
        public void ProgressBar_Decrements_By_Step(int step)
        {
            step = Math.Abs(step);
            var p = new ProgressBar(10, value: step * 2, step: step, maximum: step * 2);

            var v1 = p.Value;
            p.Decrement();
            var v2 = p.Value;
            p.Decrement();
            var v3 = p.Value;

            Assert.Equal(step * 2, v1);
            Assert.Equal(step, v2);
            Assert.Equal(0, v3);
        }

        [Fact(DisplayName = "ProgressBar returns zero length string when hidden")]
        public void ProgressBar_Returns_Zero_Length_String_When_Hidden()
        {
            var hidden = false;
            var p = new ProgressBar(10, format: new ProgressBarFormat(hiddenWhen: () => hidden));

            var s1 = p.ToString();

            hidden = true;
            var s2 = p.ToString();

            Assert.Equal(10, s1.Length);
            Assert.Equal(0, s2.Length);
        }

        [Fact(DisplayName = "ProgressBar returns empty string when empty")]
        public void ProgressBar_Returns_Empty_String_When_Empty()
        {
            var empty = false;
            var p = new ProgressBar(10, maximum: 10, value: 10, format: new ProgressBarFormat(full: '.', empty: ' ', emptyWhen: () => empty));

            var s1 = p.ToString();

            empty = true;
            var s2 = p.ToString();

            Assert.Equal("..........", s1);
            Assert.Equal("          ", s2);
        }

        [Fact(DisplayName = "ProgressBar adds formatting")]
        public void ProgressBar_Adds_Formating()
        {
            var p = new ProgressBar(1, format: new ProgressBarFormat(empty: ' ', left: "LEFT", right: "RIGHT", paddingLeft: 3, paddingRight: 2, pad: '0'));

            Assert.Equal("000LEFT RIGHT00", p.ToString());
        }
    }
}
