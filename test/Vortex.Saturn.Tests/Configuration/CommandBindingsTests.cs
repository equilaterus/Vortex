﻿
using Equilaterus.Vortex.Helpers;
using Equilaterus.Vortex.Saturn.Commands;
using Equilaterus.Vortex.Saturn.Configuration;
using Equilaterus.Vortex.Saturn.Models;
using Equilaterus.Vortex.Saturn.Queries;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests.Configuration
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

            Dictionary<string, Dictionary<string, List<SubTypeOf<VortexAction>>>> graph = null;

            graph = vortexGraph.GetType()
                .GetField("_graph", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(vortexGraph) as Dictionary<string, Dictionary<string, List<SubTypeOf<VortexAction>>>>;

            Assert.NotNull(graph);
            Assert.Equal(6, graph.Count);

            Assert.Equal(2, graph[nameof(VortexEvents.InsertEntity)].Count);
            Assert.Equal(typeof(InsertEntity<>), graph[nameof(VortexEvents.InsertEntity)]["_default"][0].TypeOf);
            Assert.Equal(typeof(InsertAttacheableFile<>), graph[nameof(VortexEvents.InsertEntity)][nameof(IAttacheableFile)][0].TypeOf);

            Assert.Equal(2, graph[nameof(VortexEvents.UpdateEntity)].Count);
            Assert.Equal(typeof(UpdateEntity<>), graph[nameof(VortexEvents.UpdateEntity)]["_default"][0].TypeOf);
            Assert.Equal(typeof(UpdateAttacheableFile<>), graph[nameof(VortexEvents.UpdateEntity)][nameof(IAttacheableFile)][0].TypeOf);

            Assert.Equal(2, graph[nameof(VortexEvents.DeleteEntity)].Count);
            Assert.Equal(typeof(DeleteEntity<>), graph[nameof(VortexEvents.DeleteEntity)]["_default"][0].TypeOf);
            Assert.Equal(typeof(DeleteAttacheableFile<>), graph[nameof(VortexEvents.DeleteEntity)][nameof(IAttacheableFile)][0].TypeOf);

            Assert.Single(graph[nameof(VortexEvents.QueryForEntities)]);
            Assert.Equal(typeof(QueryForEntities<>), graph[nameof(VortexEvents.QueryForEntities)]["_default"][0].TypeOf);
            Assert.Equal(typeof(UpdateQueryFilter<>), graph[nameof(VortexEvents.QueryForEntities)]["_default"][1].TypeOf);

            Assert.Single(graph[nameof(VortexEvents.RelationalQueryForEntities)]);
            Assert.Equal(typeof(RelationalQueryForEntities<>), graph[nameof(VortexEvents.RelationalQueryForEntities)]["_default"][0].TypeOf);
            Assert.Equal(typeof(UpdateQueryFilter<>), graph[nameof(VortexEvents.RelationalQueryForEntities)]["_default"][1].TypeOf);

            Assert.Single(graph[nameof(VortexEvents.QueryCount)]);
            Assert.Equal(typeof(QueryCount<>), graph[nameof(VortexEvents.QueryCount)]["_default"][0].TypeOf);
        }        
    }
}
