using System;
using System.Collections.Generic;
using Equilaterus.Vortex.Helpers;

namespace Equilaterus.Vortex
{
    public interface IVortexGraph
    {
        void Bind(string eventName, Type instigator, VortexBinding action);

        void CreateEvent(string eventName);

        List<VortexBinding> GetBindings(string eventName, Type instigator);
    }
}