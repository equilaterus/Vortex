
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Equilaterus.Vortex.Saturn.Queries.Filters;
using Equilaterus.Vortex.Saturn.Queries;

namespace Equilaterus.Vortex.Saturn.Tests.Queries.Filters
{
    public class SoftDeleteableFilterTests : BaseFilterTests<SoftDeleteableTestModel>
    {
        public override QueryFilter<SoftDeleteableTestModel> GetFilter()
        {
            return new SoftDeleteableFilter<SoftDeleteableTestModel>();
        }

        [Fact]
        public void UpdateFilter()
        {
            var items = new List<SoftDeleteableTestModel>()
            {
                new SoftDeleteableTestModel(){ Id = -1, IsDeleted = true },
                new SoftDeleteableTestModel(){ Id = 0, IsDeleted = true },
                new SoftDeleteableTestModel(){ Id = 1, IsDeleted = false },
                new SoftDeleteableTestModel(){ Id = 1, IsDeleted = true },
                new SoftDeleteableTestModel(){ Id = 2, IsDeleted = false },
                new SoftDeleteableTestModel(){ Id = 2, IsDeleted = true }
            };

            var activableFilter = GetFilter();

            var queryParams = 
                new QueryParams<SoftDeleteableTestModel>()
                {  Filter = e => e.Id > 0 };

            activableFilter.UpdateParams(queryParams);

            Assert.Equal(items.Where(e=> e.IsDeleted == false && e.Id > 0).Count(), items.AsQueryable().Where(queryParams.Filter).Count());
        }        
    }
}
