using Equilaterus.Vortex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vortex.Tests.Utilities
{
    public static class ExtendedVortexGraphTest
    {
        private const string GRAPH_FIELD = "_graph";

        public static Dictionary<string, Dictionary<string, List<VortexBinding>>> 
            GetGraph(this VortexGraph vortexGraph)
        {
            return vortexGraph.GetPrivateField<Dictionary<string, Dictionary<string, List<VortexBinding>>>>(GRAPH_FIELD);
        }
    }
}
