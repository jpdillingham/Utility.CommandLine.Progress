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
    }
}
