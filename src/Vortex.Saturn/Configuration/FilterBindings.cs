using Equilaterus.Vortex.Saturn.Models;
using Equilaterus.Vortex.Saturn.Queries.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Saturn.Configuration
{
    public static class FilterBindings
    {
        public static void LoadDefaults(this IGenericFilterFactory factory)
        {
            factory.Bind(typeof(IActivable), typeof(ActivableFilter<>));
            factory.Bind(typeof(ISoftDeleteable), typeof(SoftDeleteableFilter<>));
        }
    }
}
