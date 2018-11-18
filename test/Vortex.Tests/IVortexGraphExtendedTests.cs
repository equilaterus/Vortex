using Equilaterus.Vortex;
using Equilaterus.Vortex.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Vortex.Tests.Utilities;
using Xunit;

namespace Vortex.Tests
{
    public class IVortexGraphExtendedTests
    {
        internal interface ICustomInstigator : IVortexInstigator { }
        internal interface IAnotherCustomInstigator : IVortexInstigator { }

        internal class MyClass : ICustomInstigator, IAnotherCustomInstigator { }

        [Fact]
        public void Bind_Success()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();

            // Execute 
            vortexGraph.CreateEvent("event");
            vortexGraph.Bind("event", bindingMock.Object, SubTypeOf<IVortexInstigator>.GetFrom<ICustomInstigator>());

            // Check
            var result = vortexGraph.GetGraph();

            Assert.Single(result);
            Assert.Single(result["event"][typeof(ICustomInstigator).Name]);
            Assert.Equal(bindingMock.Object, result["event"][typeof(ICustomInstigator).Name][0]);
        }

        [Fact]
        public void Bind_NullInstigator_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();
            SubTypeOf< IVortexInstigator> subtypeof = null;

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => vortexGraph.Bind("event", bindingMock.Object, subtypeof));
        }

        [Fact]
        public void GetBindings_Success()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();

            // Execute 
            vortexGraph.CreateEvent("event");
            vortexGraph.Bind("event", bindingMock.Object, SubTypeOf<IVortexInstigator>.GetFrom<ICustomInstigator>());

            // Check
            var result = vortexGraph.GetBindings("event", typeof(MyClass));

            Assert.Single(result);
            Assert.Equal(bindingMock.Object, result[0]);
        }

        [Fact]
        public void GetBindings_Multiple_Success()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMockA = Mock.Of<VortexBinding>(m => m.ApplyLowerPriorityActions == true);
            var bindingMockB = Mock.Of<VortexBinding>(m => m.ApplyLowerPriorityActions == true);

            // Execute 
            vortexGraph.CreateEvent("event");
            vortexGraph.Bind("event", bindingMockA, SubTypeOf<IVortexInstigator>.GetFrom<ICustomInstigator>());
            vortexGraph.Bind("event", bindingMockB, SubTypeOf<IVortexInstigator>.GetFrom<IAnotherCustomInstigator>());

            // Check
            var result = vortexGraph.GetBindings("event", typeof(MyClass));

            Assert.Equal(2, result.Count);
            Assert.Equal(bindingMockA, result[0]);
            Assert.Equal(bindingMockB, result[1]);
        }

        [Fact]
        public void GetBindings_NullInstigator_ThrowsException()
        {
            // Prepare
            var vortexGraph = new VortexGraph();
            var bindingMock = new Mock<VortexBinding>();
            Type type = null;

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => vortexGraph.GetBindings("event", type));
        }
    }
}
