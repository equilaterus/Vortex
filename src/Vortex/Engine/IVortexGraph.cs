using System;
using System.Collections.Generic;
using Equilaterus.Vortex.Helpers;

namespace Equilaterus.Vortex.Engine
{
    public interface IVortexGraph
    {
        void Bind(string instigatorEvent, string objectInterface, SubClassOf<VortexAction> action);

        void CreateEvent(string instigatorEvent);

        List<SubClassOf<VortexAction>> GetActions(string instigatorEvent, Type typeEntity);

        List<SubClassOf<VortexAction>> GetDefaultActions(string instigatorEvent);
    }
}