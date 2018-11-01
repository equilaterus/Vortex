using System;
using Xunit;
using Equilaterus.Vortex.Helpers;
using Vortex.Tests.Utilities;
using System.Collections.Generic;

namespace Equilaterus.Vortex.Tests
{
    public class VortexGraphTests
    {
        private VortexGraph InitializeGraph()
        {
            return null;
        }

        [Fact]
        public void CreateEvent_Success()
        {
            // Prepare
            VortexGraph vortexGraph = new VortexGraph();

            // Execute
            vortexGraph.CreateEvent("event");

            // Check
            var result = vortexGraph.GetPrivateField<Dictionary<string, Dictionary<string, List<VortexBinding>>>>("_graph");

            Assert.NotEmpty(result);
            Assert.True(result.ContainsKey("event"));
            Assert.NotNull(result["event"]);
        }

        [Fact]
        public void CreateEvent_Null_ThrowsException()
        {
            // Prepare
            VortexGraph vortexGraph = new VortexGraph();

            // Execute and check
            Assert.Throws<ArgumentNullException>(() => vortexGraph.CreateEvent(null));
        }
    }
}