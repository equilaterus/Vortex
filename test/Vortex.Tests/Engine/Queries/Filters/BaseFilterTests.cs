using Equilaterus.Vortex.Engine.Queries;
using Equilaterus.Vortex.Engine.Queries.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vortex.Tests.Engine.Queries.Filters
{
    public abstract class BaseFilterTests<T> where T : class
    {
        public abstract QueryFilter<T> GetFilter();

        [Fact]
        public void NullParams()
        {
            var filter = GetFilter();
            Assert.Throws<NullReferenceException>(() => filter.UpdateParams(null));
        }

        [Fact]
        public void NullFilterParams()
        {
            var filter = GetFilter();
            Assert.Throws<NullReferenceException>(() => filter.UpdateParams(new QueryParams<T>() { Filter = null }));
        }
    }
}
