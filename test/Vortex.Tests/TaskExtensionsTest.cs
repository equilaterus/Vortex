using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Tests
{
    public class TaskExtensionsTest
    {
        private Task<string> AsyncMethod()
        {
            return Task.FromResult("Vortex");
        }

        [Fact]
        public async Task Select_Success()
        {
            // Prepare and execute
            var result = await TaskExtensions.Select(
                AsyncMethod(), str => str[0]);

            // Check
            Assert.Equal('V', result);
        }

        [Fact]
        public async Task Select_NullTask_ThrowsError()
        {
            // Execute and check
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => TaskExtensions.Select<object, object>(null, t => t)
            );
            
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public async Task Select_NullSelector_ThrowsError()
        {
            // Execute and check
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => TaskExtensions.Select<object, object>(Task.FromResult<object>(null), null)
            );

            Assert.Equal("selector", exception.ParamName);
        }

        [Fact]
        public async Task SelectMany_Success()
        {
            // Prepare and execute
            var result = await TaskExtensions.SelectMany(
                AsyncMethod(), str => Task.FromResult(str[0]));

            // Check
            Assert.Equal('V', result);
        }

        [Fact]
        public async Task SelectMany_NullTask_ThrowsError()
        {
            // Execute and check
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => TaskExtensions.SelectMany<object, object>(null, t => Task.FromResult(t))
            );

            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public async Task SelectMany_NullSelector_ThrowsError()
        {
            // Execute and check
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => TaskExtensions.SelectMany<object, object>(Task.FromResult<object>(null), null)
            );

            Assert.Equal("selector", exception.ParamName);
        }

        [Fact]
        public async Task SelectManyWithSelector_Success()
        {
            // Prepare and execute
            var result = await TaskExtensions.SelectMany(
                AsyncMethod(), str => Task.FromResult(str[0]), (str, c) => str + c);

            // Check
            Assert.Equal("VortexV", result);
        }

        [Fact]
        public async Task SelectManyWithSelector_FromNotation_Success()
        {
            // Prepare and execute
            var result = await from i in AsyncMethod()
                               from b in AsyncMethod()
                               select i + b;

            // Check
            Assert.Equal("VortexVortex", result);
        }

        [Fact]
        public async Task SelectManyWithSelector_NullTask_ThrowsError()
        {
            // Execute and check
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => TaskExtensions.SelectMany<object, object>(null, t => Task.FromResult(t))
            );

            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public async Task SelectManyWithSelector_NullSelector_ThrowsError()
        {
            // Execute and check
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => TaskExtensions.SelectMany<object, object>(Task.FromResult<object>(null), null)
            );

            Assert.Equal("selector", exception.ParamName);
        }
    }
}
