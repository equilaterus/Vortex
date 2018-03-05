using Equilaterus.Vortex.Engine.Commands;
using Equilaterus.Vortex.Engine.Queries;
using Equilaterus.Vortex.Engine.Queries.Filters;
using Equilaterus.Vortex.Helpers;
using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine.Configuration
{
    public static class CommandBindings
    {
        public static void LoadDefaults(this VortexGraph graph)
        {
            graph.CreateEvent(VortexEvents.InsertEntity);
            graph.CreateEvent(VortexEvents.UpdateEntity);
            graph.CreateEvent(VortexEvents.DeleteEntity);
            graph.CreateEvent(VortexEvents.QueryForEntities);
            graph.CreateEvent(VortexEvents.RelationalQueryForEntities);

            graph.Bind(
                nameof(VortexEvents.InsertEntity),
                "_default",
                SubClassOf<VortexAction>.GetFrom(typeof(InsertEntity<>)));

            graph.Bind(
                nameof(VortexEvents.UpdateEntity),
                "_default",
                SubClassOf<VortexAction>.GetFrom(typeof(UpdateEntity<>)));

            graph.Bind(
                nameof(VortexEvents.DeleteEntity),
                "_default",
                SubClassOf<VortexAction>.GetFrom(typeof(DeleteEntity<>)));

            graph.Bind(
                nameof(VortexEvents.QueryForEntities),
                "_default",
                SubClassOf<VortexAction>.GetFrom(typeof(QueryForEntities<>)));

            graph.Bind(
                nameof(VortexEvents.RelationalQueryForEntities),
                "_default",
                SubClassOf<VortexAction>.GetFrom(typeof(RelationalQueryForEntities<>)));

        }
    }
}
