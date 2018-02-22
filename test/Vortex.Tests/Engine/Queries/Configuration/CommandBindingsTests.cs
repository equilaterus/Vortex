using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Commands;
using Equilaterus.Vortex.Engine.Configuration;
using Equilaterus.Vortex.Engine.Queries;
using Equilaterus.Vortex.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;
using static Equilaterus.Vortex.Engine.Configuration.CommandBindings;

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
            Assert.Equal(2, graph.Count);
            Assert.Single(graph[nameof(VortexEvents.InsertEntity)]);
            Assert.Equal(typeof(InsertEntity<>), graph[nameof(VortexEvents.InsertEntity)]["_default"].TypeOf);

            Assert.Single(graph[nameof(VortexEvents.RelationalQueryForEntities)]);
            Assert.Equal(typeof(RelationalQueryForEntities<>), graph[nameof(VortexEvents.RelationalQueryForEntities)]["_default"].TypeOf);
        }        
    }
}
