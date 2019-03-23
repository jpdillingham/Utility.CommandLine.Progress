﻿/*
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
    using System.Collections.Generic;
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

        [Fact(DisplayName = "Spinner.Spin advances all frames in order")]
        public void Spinner_Spin_Advances_All_Frames_In_Order()
        {
            var frames = new List<string>();

            var s = new Spinner('a', 'b', 'c');

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

        [Fact(DisplayName = "Spinner respects EmptyWhen")]
        public void Spinner_Respects_EmptyWhen()
        {
            var empty = false;
            var s = new Spinner(new SpinnerFormat(emptyWhen: () => empty));

            var empty1 = s.ToString();

            empty = true;
            var empty2 = s.ToString();

            Assert.NotEqual(s.Format.Empty.ToString(), empty1);
            Assert.Equal(s.Format.Empty.ToString(), empty2);
        }

        [Fact(DisplayName = "Spinner respects HiddenWhen")]
        public void Spinner_Respects_HiddenWhen()
        {
            var hidden = false;
            var s = new Spinner(new SpinnerFormat(hiddenWhen: () => hidden));

            var hidden1 = s.ToString();

            hidden = true;
            var hidden2 = s.ToString();

            Assert.NotEqual(string.Empty, hidden1);
            Assert.Equal(string.Empty, hidden2);
        }

        [Fact(DisplayName = "Spinner returns formatted output")]
        public void Spinner_Returns_Formatted_Output()
        {
            var s = new Spinner(new SpinnerFormat(left: "[", right: "]", paddingLeft: 1, paddingRight: 1, pad: '.'));

            Assert.Equal(".[-].", s.ToString());
        }
    }
}
