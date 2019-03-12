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
    }
}
