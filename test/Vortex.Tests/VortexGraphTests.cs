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
            vortexGraph.Bind("event", bindingMock.Object, "instigator");

            // Check
            var result = vortexGraph.GetGraph();

            Assert.Single(result);
            Assert.Single(result["event"]["instigator"]);
            Assert.Equal(bindingMock.Object, result["event"]["instigator"][0]);
        }

        [Fact]
        public void Bind_NullInstigatorType_Success()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();

            // Execute
            vortexGraph.CreateEvent("event");
            vortexGraph.Bind("event", bindingMock.Object);

            // Check
            var result = vortexGraph.GetGraph();

            Assert.Single(result);
            Assert.Single(result["event"][VortexGraph.DEFAULT_INSTIGATOR]);
            Assert.Equal(bindingMock.Object, result["event"][VortexGraph.DEFAULT_INSTIGATOR][0]);
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
            var bindingMockE = new Mock<VortexBinding>();
            
            // Execute 
            vortexGraph.CreateEvent("event");
            vortexGraph.CreateEvent("event-2");
            vortexGraph.Bind("event", bindingMockA.Object, "object");
            vortexGraph.Bind("event", bindingMockB.Object, "object");
            vortexGraph.Bind("event", bindingMockC.Object, "int");
            vortexGraph.Bind("event-2", bindingMockD.Object, "object");
            vortexGraph.Bind("event-2", bindingMockE.Object);

            // Check
            var result = vortexGraph.GetGraph();            

            // Two events
            Assert.Equal(2, result.Count);

            // First event
            // Check first event for object
            Assert.Equal(2, result["event"]["object"].Count);
            Assert.Equal(bindingMockA.Object, result["event"]["object"][0]);
            Assert.Equal(bindingMockB.Object, result["event"]["object"][1]);
            // Check first event for int
            Assert.Single(result["event"]["int"]);
            Assert.Equal(bindingMockC.Object, result["event"]["int"][0]);

            // Second event
            // Check second event for object
            Assert.Single(result["event-2"]["object"]);
            Assert.Equal(bindingMockD.Object, result["event-2"]["object"][0]);
            // Check second event for default (null instigator)
            Assert.Single(result["event-2"][VortexGraph.DEFAULT_INSTIGATOR]);
            Assert.Equal(bindingMockE.Object, result["event-2"][VortexGraph.DEFAULT_INSTIGATOR][0]);
        }

        [Fact]
        public void Bind_UnexistingEvent_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();

            // Execute and check
            Assert.Throws<Exception>(
                () => vortexGraph.Bind("event", bindingMock.Object, "object"));
        }

        [Fact]
        public void Bind_NullEventName_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => vortexGraph.Bind(null, bindingMock.Object, "object"));
        }       

        [Fact]
        public void Bind_NullBinding_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => vortexGraph.Bind(string.Empty, null, "object"));
        }

        [Fact]
        public void GetBindings_NoBindings_Sucess()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            vortexGraph.CreateEvent("event");

            // Execute
            var result = vortexGraph.GetBindings("event", "object");

            // Check
            Assert.Empty(result);
        }

        [Fact]
        public void GetBindings_NullInstigator_Success()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            vortexGraph.CreateEvent("event");

            // Execute
            var result = vortexGraph.GetBindings("event");

            // Check
            Assert.Empty(result);
        }

        [Fact]
        public void GetBindings_Sucess()
        {
            // Prepare
            var bindingMock = new Mock<VortexBinding>();
            var vortexGraph = new VortexGraph();            
            vortexGraph.CreateEvent("event");
            vortexGraph.Bind("event", bindingMock.Object, "object");

            // Execute
            var result = vortexGraph.GetBindings("event", "object");

            // Check
            Assert.Single(result);
            Assert.Equal(bindingMock.Object, result[0]);
        }

        // Priority, 
        [Fact]
        public void GetBindings_MultipleHandlers_PriorityOrdered_Sucess()
        {
            // Prepare
            var mock_eventA_null = new Mock<VortexBinding>();
            var mock_eventA_int = Mock.Of<VortexBinding>(
                m => m.ApplyLowerPriorityActions == true && m.Priority == 100
            );
            var mock_eventB_null = new Mock<VortexBinding>();
            var mock_eventB_int = Mock.Of<VortexBinding>(
                m => m.ApplyLowerPriorityActions == false && m.Priority == 100
            );
            var vortexGraph = new VortexGraph();
            vortexGraph.CreateEvent("eventA");
            vortexGraph.CreateEvent("eventB");
            vortexGraph.Bind("eventA", mock_eventA_null.Object);
            vortexGraph.Bind("eventA", mock_eventA_int, "int");
            vortexGraph.Bind("eventB", mock_eventB_null.Object);
            vortexGraph.Bind("eventB", mock_eventB_int, "int");

            // Execute
            var resultA = vortexGraph.GetBindings("eventA");
            var resultAint = vortexGraph.GetBindings("eventA", "int");
            var resultB = vortexGraph.GetBindings("eventB");
            var resultBint = vortexGraph.GetBindings("eventB", "int");

            // Check A
            Assert.Single(resultA);
            Assert.Equal(mock_eventA_null.Object, resultA[0]);

            // Check A for instigator int
            Assert.Equal(2, resultAint.Count);
            Assert.Equal(mock_eventA_int, resultAint[0]);
            Assert.Equal(mock_eventA_null.Object, resultAint[1]);

            // Check B
            Assert.Single(resultB);
            Assert.Equal(mock_eventB_null.Object, resultB[0]);

            // Check B for instigator int (it has ApplyLowerPriorityActions == false)
            Assert.Single(resultBint);
            Assert.Equal(mock_eventB_int, resultBint[0]);
        }

        [Fact]
        public void GetBindings_NullEvent_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => vortexGraph.GetBindings(null, null));
        }        

        [Fact]
        public void GetBindings_UndefinedEvent_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();

            // Execute and check
            Assert.Throws<Exception>(
                () => vortexGraph.GetBindings("event", "object"));
        }
    }
}