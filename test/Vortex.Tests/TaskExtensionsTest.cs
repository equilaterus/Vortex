using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Vortex.Tests
{
    public class TaskExtensionsTest
    {
        [Fact]
        public async Task Select_NullTask_ThrowsError()
        {
            // Execute and check
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => Equilaterus.Vortex.TaskExtensions.Select<object, object>(null, t => t)
            );
            
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public async Task Select_NullSelector_ThrowsError()
        {
            // Execute and check
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => Equilaterus.Vortex.TaskExtensions.Select<object, object>(Task.FromResult<object>(null), null)
            );

            Assert.Equal("selector", exception.ParamName);
        }
    }
}
