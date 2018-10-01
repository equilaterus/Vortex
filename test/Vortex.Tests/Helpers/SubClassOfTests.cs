using System;
using System.IO;
using Xunit;

namespace Equilaterus.Vortex.Helpers.Test
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
