using System;
using Xunit;
using Equilaterus.Vortex.Helpers;
using Vortex.Tests.Utilities;
using System.Collections.Generic;
using Moq;

namespace Equilaterus.Vortex.Tests
{
    public class VortexGraphTests
    {
        [Fact]
        public void CreateEvent_Success()
        {
            // Prepare
            var vortexGraph = new VortexGraph();

            // Execute
            vortexGraph.CreateEvent("event");

            // Check
            var result = vortexGraph.GetGraph();

            Assert.NotEmpty(result);
            Assert.True(result.ContainsKey("event"));
            Assert.NotNull(result["event"]);
        }

        [Fact]
        public void CreateEvent_Null_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => vortexGraph.CreateEvent(null));
        }

        [Fact]
        public void CreateEvent_DuplicatedKey_ThrowsException()
        {
            // Prepare
            VortexGraph vortexGraph = new VortexGraph();

            // Execute
            vortexGraph.CreateEvent("event");

            // Execute and check
            Assert.Throws<Exception>(() => vortexGraph.CreateEvent("event"));
        }

        [Fact]
        public void Bind_Success()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();

            // Execute 
            vortexGraph.CreateEvent("event");
            vortexGraph.Bind("event", typeof(object), bindingMock.Object);

            // Check
            var result = vortexGraph.GetGraph();

            Assert.Single(result);
            Assert.Single(result["event"][typeof(object).ToString()]);
            Assert.Equal(bindingMock.Object, result["event"][typeof(object).ToString()][0]);
        }

        [Fact]
        public void Bind_MultipleHandlers_Success()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMockA = new Mock<VortexBinding>();
            var bindingMockB = new Mock<VortexBinding>();
            var bindingMockC = new Mock<VortexBinding>();
            var bindingMockD = new Mock<VortexBinding>();

            // Preconditions
            Assert.NotEqual(bindingMockA.Object, bindingMockB.Object);
            Assert.NotEqual(bindingMockB.Object, bindingMockC.Object);

            // Execute 
            vortexGraph.CreateEvent("event");
            vortexGraph.CreateEvent("event-2");
            vortexGraph.Bind("event", typeof(object), bindingMockA.Object);
            vortexGraph.Bind("event", typeof(object), bindingMockB.Object);
            vortexGraph.Bind("event", typeof(int), bindingMockC.Object);
            vortexGraph.Bind("event-2", typeof(object), bindingMockD.Object);

            // Check
            var result = vortexGraph.GetGraph();            

            Assert.Equal(2, result.Count);
            Assert.Equal(2, result["event"][typeof(object).ToString()].Count);
            Assert.Equal(bindingMockA.Object, result["event"][typeof(object).ToString()][0]);
            Assert.Equal(bindingMockB.Object, result["event"][typeof(object).ToString()][1]);
            Assert.Single(result["event"][typeof(int).ToString()]);
            Assert.Equal(bindingMockC.Object, result["event"][typeof(int).ToString()][0]);
            Assert.Single(result["event-2"][typeof(object).ToString()]);
            Assert.Equal(bindingMockD.Object, result["event-2"][typeof(object).ToString()][0]);
        }

        [Fact]
        public void Bind_UnexistingEvent_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();

            // Execute and check
            Assert.Throws<Exception>(
                () => vortexGraph.Bind("event", typeof(object), bindingMock.Object));
        }

        [Fact]
        public void Bind_NullEventName_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => vortexGraph.Bind(null, typeof(object), bindingMock.Object));
        }

        [Fact]
        public void Bind_NullInstigatorType_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => vortexGraph.Bind(string.Empty, null, bindingMock.Object));
        }

        [Fact]
        public void Bind_NullBinding_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => vortexGraph.Bind(string.Empty, typeof(object), null));
        }
    }
}