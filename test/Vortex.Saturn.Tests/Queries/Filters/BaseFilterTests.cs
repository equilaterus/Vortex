using Equilaterus.Vortex.Saturn.Queries;
using Equilaterus.Vortex.Saturn.Queries.Filters;
using System;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests.Queries.Filters
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
            var queryParams = new QueryParams<T>() { Filter = null };
            filter.UpdateParams(queryParams);
            Assert.NotNull(queryParams.Filter);
        }
    }
}
