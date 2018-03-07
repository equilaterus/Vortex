using Equilaterus.Vortex.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Vortex.Tests.Helpers
{
    public class SubClassOfTests
    {
        [Fact]
        public void SubClassOfValid()
        {
            var subclass = SubClassOf<Stream>.GetFrom(typeof(FileStream));

            Assert.Equal(typeof(FileStream), subclass.TypeOf);
        }

        [Fact]
        public void SubClassOfInvalid()
        {
            Assert.Throws<Exception>(
                () => SubClassOf<string>.GetFrom(typeof(float))
            );
        }
    }
}
