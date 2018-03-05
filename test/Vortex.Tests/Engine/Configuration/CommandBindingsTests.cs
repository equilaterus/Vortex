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

namespace Vortex.Tests.Engine.Configuration
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
            Assert.Equal(5, graph.Count);

            Assert.Single(graph[nameof(VortexEvents.InsertEntity)]);
            Assert.Equal(typeof(InsertEntity<>), graph[nameof(VortexEvents.InsertEntity)]["_default"].TypeOf);

            Assert.Single(graph[nameof(VortexEvents.UpdateEntity)]);
            Assert.Equal(typeof(UpdateEntity<>), graph[nameof(VortexEvents.UpdateEntity)]["_default"].TypeOf);

            Assert.Single(graph[nameof(VortexEvents.DeleteEntity)]);
            Assert.Equal(typeof(DeleteEntity<>), graph[nameof(VortexEvents.DeleteEntity)]["_default"].TypeOf);

            Assert.Single(graph[nameof(VortexEvents.QueryForEntities)]);
            Assert.Equal(typeof(QueryForEntities<>), graph[nameof(VortexEvents.QueryForEntities)]["_default"].TypeOf);

            Assert.Single(graph[nameof(VortexEvents.RelationalQueryForEntities)]);
            Assert.Equal(typeof(RelationalQueryForEntities<>), graph[nameof(VortexEvents.RelationalQueryForEntities)]["_default"].TypeOf);
        }        
    }
}
