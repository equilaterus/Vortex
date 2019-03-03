using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Equilaterus.Vortex.Tests
{
    public class Maybe
    {
        [Fact]
        public void Create_Null_ThrowsError()
        {
            Assert.Throws<ArgumentNullException>(
                () => { Maybe<object> maybe = new Maybe<object>(null); });
		}
    }
}
