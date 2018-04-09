using Equilaterus.Vortex.Engine.Configuration;
using Equilaterus.Vortex.Engine.Queries.Filters;
using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace Vortex.Tests.Engine.Configuration
{
    public class FilterBindingTests
    {
        [Fact]
        public void TestBindings()
        {
            GenericFilterFactory filterFactory = new GenericFilterFactory();
            filterFactory.LoadDefaults();

            Dictionary<Type, List<Type>> bindings = null;

            bindings = filterFactory.GetType()
                .GetField("_bindings", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(filterFactory) as Dictionary<Type, List<Type>>;

            Assert.NotNull(bindings);

            Assert.True(2 == bindings.Count);
            Assert.True(1 == bindings[typeof(IActivable)].Count);
            Assert.True(typeof(ActivableFilter<>) == bindings[typeof(IActivable)][0]);

            Assert.True(1 == bindings[typeof(ISoftDeleteable)].Count);
            Assert.True(typeof(SoftDeleteableFilter<>) == bindings[typeof(ISoftDeleteable)][0]);
        }
    }
}
