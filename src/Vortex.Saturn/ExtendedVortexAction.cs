using Equilaterus.Vortex.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Saturn
{
    public static class ExtendedVortexAction
    {
        public static VortexContext<T> GetContext<T>(this VortexAction<T> vortexAction) where T : class
        {
            return vortexAction.Context as VortexContext<T>;
        }
    }
}
