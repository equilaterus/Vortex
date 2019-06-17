using Equilaterus.Vortex;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Equilaterus.Vortex.Tests
{
    public class MaybeExtensionTest
    {
        [Fact]
        public void MatchBool_False()
        {
            // Prepare
            Maybe<bool> maybe = new Maybe<bool>();

            // Execute
            var result = maybe.MatchBool(x => x == true, false);

            // Check
            Assert.False(result);
        }

        [Fact]
        public void MatchBool_true()
        {
            // Prepare
            Maybe<bool> maybe = new Maybe<bool>(true);

            // Execute
            var result = maybe.MatchBool(x => x == true, false);

            // Check
            Assert.True(result);
        }
    }
}
