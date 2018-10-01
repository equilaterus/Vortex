using Equilaterus.Vortex.Saturn.Configuration;
using Equilaterus.Vortex.Saturn.Models;
using Equilaterus.Vortex.Saturn.Queries.Filters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests.Configuration
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
