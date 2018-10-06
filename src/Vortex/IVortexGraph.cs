using System;
using System.Collections.Generic;
using Equilaterus.Vortex.Helpers;

namespace Equilaterus.Vortex
{
    public interface IVortexGraph
    {
        void Bind(string instigatorEvent, string objectInterface, SubTypeOf<VortexAction> action);

        void CreateEvent(string instigatorEvent);

        List<SubTypeOf<VortexAction>> GetActions(string instigatorEvent, Type typeEntity);

        List<SubTypeOf<VortexAction>> GetDefaultActions(string instigatorEvent);
    }
}