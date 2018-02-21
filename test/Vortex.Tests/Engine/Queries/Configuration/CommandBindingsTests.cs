using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Configuration;
using Equilaterus.Vortex.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace Vortex.Tests.Engine.Queries.Configuration
{
    public class CommandBindingsTests
    {
        class MyModel
        {
            public int Id { get; set; }
        }

        [Fact]
        public void LoadDefaultCommands()
        {
            VortexGraph vortexGraph = new VortexGraph();
            vortexGraph.LoadDefaults();

            Dictionary<string, Dictionary<string, SubClassOf<VortexAction>>> graph = null;

            graph = vortexGraph.GetType()
                .GetField("_graph", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(vortexGraph) as Dictionary<string, Dictionary<string, SubClassOf<VortexAction>>>;

            Assert.NotNull(graph);
            
        }
        
    }
}
