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
        public void Create_Nothing_Success()
        {
            // Prepare and execute
            Maybe<object> maybe = new Maybe<object>();

            // Check
            Assert.False(maybe.GetPrivateField<bool>(HAS_VALUE_FIELD));
            Assert.Null(maybe.GetPrivateField<object>(VALUE_FIELD));
        }

        [Fact]
        public void Create_Success()
        {
            // Prepare and execute
            var obj = new object();
            Maybe<object> maybe = new Maybe<object>(obj);
            
            // Check
            Assert.True(maybe.GetPrivateField<bool>(HAS_VALUE_FIELD));
            Assert.Equal(obj, maybe.GetPrivateField<object>(VALUE_FIELD));
        }

        [Fact]
        public void Create_Null_ThrowsError()
        {
            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => { Maybe<object> maybe = new Maybe<object>(null); });
		}

        [Fact]
        public void Select_Null_ThrowsError()
        {
            // Prepare
            Maybe<object> maybe = new Maybe<object>();

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => { maybe.Select<object>(null); });
        }

        [Fact]
        public void Select_Nothing_Success()
        {
            // Prepare
            Maybe<object> maybe = new Maybe<object>();

            // Execute
            var result = maybe.Select(m => m);

            // Check
            Assert.NotEqual(maybe, result);
            Assert.False(result.GetPrivateField<bool>(HAS_VALUE_FIELD));
            Assert.Null(result.GetPrivateField<object>(VALUE_FIELD));
        }

        [Fact]
        public void Select_Success()
        {
            // Prepare
            Maybe<int> maybe = new Maybe<int>(5);

            // Execute
            var result = maybe.Select(m => m * 2);

            // Check
            Assert.NotEqual(maybe, result);
            Assert.True(result.GetPrivateField<bool>(HAS_VALUE_FIELD));
            Assert.Equal(10, result.GetPrivateField<int>(VALUE_FIELD));
        }

        [Fact]
        public void Select_FuncReturnsNull_ThrowsError()
        {
            // Prepare
            Maybe<int> maybe = new Maybe<int>(5);

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => maybe.Select<int?>(m => null));
        }

        [Fact]
        public void SelectMany_Null_ThrowsError()
        {
            // Prepare
            Maybe<object> maybe = new Maybe<object>();

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => { maybe.SelectMany<object>(null); });
        }

        [Fact]
        public void SelectMany_Nothing_Success()
        {
            // Prepare
            Maybe<object> maybe = new Maybe<object>();

            // Execute
            var result = maybe.SelectMany(m => new Maybe<object>(new object()));

            // Check
            Assert.NotEqual(maybe, result);
            Assert.False(result.GetPrivateField<bool>(HAS_VALUE_FIELD));
            Assert.Null(result.GetPrivateField<object>(VALUE_FIELD));
        }

        [Fact]
        public void SelectMany_Success()
        {
            // Prepare
            Maybe<int> maybe = new Maybe<int>(5);

            // Execute
            var result = maybe.SelectMany(m => new Maybe<double>(m * 2));

            // Check
            Assert.True(result.GetPrivateField<bool>(HAS_VALUE_FIELD));
            Assert.Equal(10, result.GetPrivateField<double>(VALUE_FIELD));
        }

        [Fact]
        public void SelectMany_FuncReturnsNull_ThrowsError()
        {
            // Prepare
            Maybe<int> maybe = new Maybe<int>(5);

            // Execute and check
            Assert.Null(maybe.SelectMany<int>(m => null));
        }

        [Fact]
        public void Match_Success()
        {
            // Prepare
            Maybe<int> maybe = new Maybe<int>(5);

            // Execute
            var result = maybe.Match(c => c, 0);

            // Check
            Assert.Equal(5, result);
        }

        [Fact]
        public void Match__Nothing_Success()
        {
            // Prepare
            Maybe<int> maybe = new Maybe<int>();

            // Execute
            var result = maybe.Match(c => c, 0);

            // Check
            Assert.Equal(0, result);
        }

        [Fact]
        public void Match_JustNull_ThrowsError()
        {
            // Prepare
            Maybe<int> maybe = new Maybe<int>(5);

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => { maybe.Match(null, 0); });
        }
    }
}
