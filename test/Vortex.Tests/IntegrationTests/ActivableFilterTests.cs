using Equilaterus.Vortex.ModelActions;
using Equilaterus.Vortex.Models;
using System;
using Xunit;

namespace Vortex.Tests.IntegrationTests
{
    public class ActivableFilterTests
    {
        private class ActivableTestModel : IActivable
        {
            public bool IsActive { get; set; }

            public int Counter { get; set; }
        }

        [Fact]
        public void ActivableFilterTests_DoFilter()
        {
            var activableFilter = new ActivableFilter<ActivableTestModel>();

            var result = activableFilter.Do(e => e.Counter > 0);

            Assert.NotNull(result);

            // Test against EF
            Assert.NotNull(null);
        }
    }
}
