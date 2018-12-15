﻿using System;
using System.IO;
using Equilaterus.Vortex.Helpers;
using Xunit;

namespace Equilaterus.Vortex.Tests.Helpers
{
    public class SubTypeOfTests
    {
        [Fact]
        public void SubTypeOfValid()
        {
            var subtype = SubTypeOf<Stream>.GetFrom(typeof(FileStream));

            Assert.Equal(typeof(FileStream), subtype.TypeOf);
        }

        [Fact]
        public void SubTypeOfInvalid()
        {
            Assert.Throws<Exception>(
                () => SubTypeOf<string>.GetFrom(typeof(float))
            );
        }
    }
}
