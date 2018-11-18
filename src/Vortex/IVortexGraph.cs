using System;
using System.Collections.Generic;
using Equilaterus.Vortex.Helpers;

namespace Equilaterus.Vortex
{
    public interface IVortexGraph
    {
        void Bind(string eventName, VortexBinding action, string instigator = null);

        void CreateEvent(string eventName);

        List<VortexBinding> GetBindings(string eventName, params string[] instigators);
    }
}