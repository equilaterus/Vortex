using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine
{
    public class VortexExecutor<T> where T : class
    {
        protected VortexContext<T> _context;

        protected VortexGraph _graph;

        public VortexExecutor(VortexContext<T> context, VortexGraph graph)
        {
            _context = context;
            _graph = graph;
        }

        public void Execute(string vortexEvent, ActionParams actionParams)
        {
            var actions = _graph.GetActions(vortexEvent, typeof(T));
            //actions.Sort(a => a.)
        }
    }
}
