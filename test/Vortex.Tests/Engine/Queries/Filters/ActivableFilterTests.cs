﻿using Equilaterus.Vortex.Models;
using Equilaterus.Vortex.Engine.Queries.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Equilaterus.Vortex.Engine.Configuration;
using Equilaterus.Vortex.Engine.Queries;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace Vortex.Tests.Engine.Queries.Filters
{
    public class ActivableFilterTests : BaseFilterTests<ActivableTestModel>
    {
        public override QueryFilter<ActivableTestModel> GetFilter()
        {
            return new ActivableFilter<ActivableTestModel>();
        }

        [Fact]
        public void UpdateFilter()
        {
            var items = new List<ActivableTestModel>()
            {
                new ActivableTestModel(){ Id = -1, IsActive = true },
                new ActivableTestModel(){ Id = 0, IsActive = true },
                new ActivableTestModel(){ Id = 1, IsActive = false },
                new ActivableTestModel(){ Id = 1, IsActive = true },
                new ActivableTestModel(){ Id = 2, IsActive = false },
                new ActivableTestModel(){ Id = 2, IsActive = true }
            };

            var activableFilter = GetFilter();

            var queryParams = 
                new QueryParams<ActivableTestModel>()
                {  Filter = e => e.Id > 0 };

            activableFilter.UpdateParams(queryParams);

            Assert.Equal(items.Where(e=> e.IsActive && e.Id > 0).Count(), items.AsQueryable().Where(queryParams.Filter).Count());
        }

        
    }
}
