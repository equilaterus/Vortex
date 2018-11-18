using Equilaterus.Vortex.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Equilaterus.Vortex
{
    public static class IVortexGraphExtended
    {
        public static void Bind(this IVortexGraph vortexGraph, string eventName, VortexBinding action, SubTypeOf<IVortexInstigator> instigator)
        {
            if (instigator == null)
            {
                throw new ArgumentNullException(nameof(instigator));
            }

            vortexGraph.Bind(eventName, action, instigator.TypeOf.Name);
        }

        public static List<VortexBinding> GetBindings(this IVortexGraph vortexGraph, string eventName, Type instigator)
        {
            if (instigator == null)
            {
                throw new ArgumentNullException(nameof(instigator));
            }

            var implementedInterfaces = instigator.GetInterfaces().Select(i => i.Name).ToList();
            implementedInterfaces.Remove(nameof(IVortexInstigator));
            return vortexGraph.GetBindings(eventName, implementedInterfaces.ToArray());
        }
    }
}
