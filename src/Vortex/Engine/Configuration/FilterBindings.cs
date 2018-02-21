using Equilaterus.Vortex.Engine.Queries.Filters;
using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine.Configuration
{
    public static class FilterBindings
    {
        public static void LoadDefaults(this GenericFilterFactory factory)
        {
            factory.Bind(typeof(IActivable), typeof(ActivableFilter<>));
            factory.Bind(typeof(ISoftDeleteable), typeof(SoftDeleteableFilter<>));
        }
    }
}
