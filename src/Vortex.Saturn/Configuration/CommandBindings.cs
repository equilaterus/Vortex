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
        }
    }
}
