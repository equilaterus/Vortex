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
            graph.CreateEvent(VortexEvents.InsertEntity.ToString());

            graph.Bind(
                nameof(VortexEvents.InsertEntity),
                "_default",
                SubClassOf<VortexAction>.GetFrom(typeof(InsertEntity<>)));


            graph.CreateEvent(VortexEvents.RelationalQueryForEntities.ToString());
            graph.Bind(
                nameof(VortexEvents.RelationalQueryForEntities),
                "_default",
                SubClassOf<VortexAction>.GetFrom(typeof(RelationalQueryForEntities<>)));

        }
    }
}
