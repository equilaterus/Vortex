using System;
using System.Collections.Generic;
using System.Text;
using Vortex.Tests.Shared;
using Xunit;

namespace Equilaterus.Vortex.Tests
{
    public class Maybe
    {
        const string HAS_VALUE_FIELD = "_hasValue";
        const string VALUE_FIELD = "_value";

        [Fact]
        public void Create_Empty_Success()
        {
            Maybe<object> maybe = new Maybe<object>();

            Assert.False(maybe.GetPrivateField<bool>(HAS_VALUE_FIELD));
            Assert.Null(maybe.GetPrivateField<object>(VALUE_FIELD));
        }

        [Fact]
        public void Create_Null_ThrowsError()
        {
            Assert.Throws<ArgumentNullException>(
                () => { Maybe<object> maybe = new Maybe<object>(null); });
		}
    }
}
