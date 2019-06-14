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

        [Fact]
        public static void AsMaybeString_Null_Returns_Nothing()
        {
            // Validating with a different data-type 
            // Prepare
            string str = null;

            // Execute
            Maybe<string> maybe = str.AsMaybe();
            var result = maybe.Match(x => x, string.Empty);

            // Check
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public static void AsMaybe_Returns_Just()
        {
            // Prepare
            int? number = 700;

            // Execute
            Maybe<int?> maybe = number.AsMaybe();
            var result = maybe.Match(x => x, -1);

            // Check
            Assert.Equal(700, result);
        }

        [Fact]
        public static void AsMaybe_Object_Returns_Just()
        {
            // Using polymorphism
            // Prepare
            string str = "Vortex!";

            // Execute
            Maybe<object> maybe = str.AsMaybe<object>();
            var result = maybe.Match(x => x, -1);

            // Check
            Assert.Equal("Vortex!", result);
        }
    }
}
