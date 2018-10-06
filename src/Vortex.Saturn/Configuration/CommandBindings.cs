using Equilaterus.Vortex.Helpers;
using Equilaterus.Vortex.Saturn.Commands;
using Equilaterus.Vortex.Saturn.Models;
using Equilaterus.Vortex.Saturn.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Saturn.Configuration
{
    public static class CommandBindings
    {
        public static void LoadDefaults(this IVortexGraph graph)
        {
            graph.CreateEvent(VortexEvents.InsertEntity);
            graph.CreateEvent(VortexEvents.UpdateEntity);
            graph.CreateEvent(VortexEvents.DeleteEntity);
            graph.CreateEvent(VortexEvents.QueryForEntities);
            graph.CreateEvent(VortexEvents.RelationalQueryForEntities);
            graph.CreateEvent(VortexEvents.QueryCount);

            graph.Bind(
                nameof(VortexEvents.InsertEntity),
                "_default",
                SubTypeOf<VortexAction>.GetFrom(typeof(InsertEntity<>)));

            graph.Bind(
                nameof(VortexEvents.UpdateEntity),
                "_default",
                SubTypeOf<VortexAction>.GetFrom(typeof(UpdateEntity<>)));

            graph.Bind(
                nameof(VortexEvents.DeleteEntity),
                "_default",
                SubTypeOf<VortexAction>.GetFrom(typeof(DeleteEntity<>)));

            graph.Bind(
                nameof(VortexEvents.QueryForEntities),
                "_default",
                SubTypeOf<VortexAction>.GetFrom(typeof(QueryForEntities<>)));

            graph.Bind(
                nameof(VortexEvents.QueryForEntities),
                "_default",
                SubTypeOf<VortexAction>.GetFrom(typeof(UpdateQueryFilter<>)));

            graph.Bind(
                nameof(VortexEvents.RelationalQueryForEntities),
                "_default",
                SubTypeOf<VortexAction>.GetFrom(typeof(RelationalQueryForEntities<>)));

            graph.Bind(
               nameof(VortexEvents.RelationalQueryForEntities),
               "_default",
               SubTypeOf<VortexAction>.GetFrom(typeof(UpdateQueryFilter<>)));


            // Adjuntables
            graph.Bind(
                nameof(VortexEvents.InsertEntity),
                nameof(IAttacheableFile),
                SubTypeOf<VortexAction>.GetFrom(typeof(InsertAttacheableFile<>)));

            graph.Bind(
                nameof(VortexEvents.UpdateEntity),
                nameof(IAttacheableFile),
                SubTypeOf<VortexAction>.GetFrom(typeof(UpdateAttacheableFile<>)));

            graph.Bind(
                nameof(VortexEvents.DeleteEntity),
                nameof(IAttacheableFile),
                SubTypeOf<VortexAction>.GetFrom(typeof(DeleteAttacheableFile<>)));

            graph.Bind(
                nameof(VortexEvents.QueryCount),
                "_default",
                SubTypeOf<VortexAction>.GetFrom(typeof
                  (QueryCount<>)));
        }
    }
}
