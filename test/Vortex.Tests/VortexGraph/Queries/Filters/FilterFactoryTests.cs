using Equilaterus.Vortex.Models;
using Equilaterus.Vortex.VortexGraph.Queries.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vortex.Tests.VortexGraph.Queries.Filters
{
    public class FilterFactoryTests
    {
        class MyModel
        {
            public int Id { get; set; }
        }


        class ActiveModel : MyModel, IActivable
        {
            public bool IsActive { get; set; }
        }

        [Fact]
        public void GetFilters()
        {
            IFilterFactory<ActiveModel> filterFactory = new GenericFilterFactory<ActiveModel>();

            var result = filterFactory.GetFilters();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result[0] is ActivableFilter<ActiveModel>);
        }
    }
}
