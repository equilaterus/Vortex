using Equilaterus.Vortex.Models;
using Equilaterus.Vortex.Engine.Queries.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vortex.Tests.Engine.Queries.Filters
{
    public class GenericFilterFactoryTests
    {
        class MyModel
        {
            public int Id { get; set; }
        }


        class ActiveModel : MyModel, IActivable
        {
            public bool IsActive { get; set; }
        }

        class ActiveSoftDeleteableModel : MyModel, IActivable, ISoftDeleteable
        {
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
        }

        [Fact]
        public void GetFilterForVanilla()
        {
            IFilterFactory<MyModel> filterFactory = new GenericFilterFactory<MyModel>();

            var result = filterFactory.GetFilters();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetFilterForActivable()
        {
            IFilterFactory<ActiveModel> filterFactory = new GenericFilterFactory<ActiveModel>();

            var result = filterFactory.GetFilters();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result[0] is ActivableFilter<ActiveModel>);
        }

        [Fact]
        public void GetFilterForActivableAndSoftDeleteable()
        {
            IFilterFactory<ActiveSoftDeleteableModel> filterFactory = new GenericFilterFactory<ActiveSoftDeleteableModel>();

            var result = filterFactory.GetFilters();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result[0] is ActivableFilter<ActiveSoftDeleteableModel>);
            Assert.True(result[1] is SoftDeleteableFilter<ActiveSoftDeleteableModel>);
        }
    }
}
