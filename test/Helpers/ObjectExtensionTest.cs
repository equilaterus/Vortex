using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Equilaterus.Vortex.Helpers.Tests
{
    public class ObjectExtensionTest
    {
        [Fact]
        public static void AsMaybe_Null_Returns_Nothing()
        {
            // Prepare
            int? str = null;

            // Execute
            Maybe<int?> maybe = str.AsMaybe();
            var result = maybe.Match(x => x, -1);

            // Check
            Assert.Equal(-1, result);
        }
    }
}
