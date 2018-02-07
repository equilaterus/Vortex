using Equilaterus.Vortex.Models;
using Equilaterus.Vortex.Services.EFCore;
using Equilaterus.Vortex.Services.MongoDB;
using Equilaterus.Vortex.VortexGraph.Queries;
using Equilaterus.Vortex.VortexGraph.Queries.Filters;
using Microsoft.EntityFrameworkCore;
using Mongo2Go;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Tests.IntegrationTests
{
    public class ActivableTestModel : MongoDbEntity, IActivable
    {
        public bool IsActive { get; set; }
        public int Counter { get; set; }

        public ActivableTestModel() : base() { }
    }       

    public class ActivableFilterTests : IntegrationTest<ActivableTestModel>
    {
        protected override List<ActivableTestModel> GetSeed<ActivableTestModel>()
        {
            return new List<ActivableTestModel>()
                {
                    new ActivableTestModel { IsActive = true, Counter = 1},
                    new ActivableTestModel { IsActive = false, Counter = 1},
                    new ActivableTestModel { IsActive = true, Counter = 0},
                    new ActivableTestModel { IsActive = false, Counter = 0},
                    new ActivableTestModel { IsActive = true, Counter = 1}
                };
        }

        private void Check(List<ActivableTestModel> result)
        {
            Assert.Equal(2, result.Count);
            foreach (var entity in result)
            {
                Assert.True(entity.IsActive);
                Assert.True(entity.Counter > 0);
            }
        }

        [Fact]
        public async Task ActivableFilterTestsEFCore()
        {            
            var dbName = nameof(ActivableFilterTestsEFCore);

            using (var context = GetContext(dbName))
            {
                await context.AddRangeAsync(GetSeed<ActivableTestModel>());
                await context.SaveChangesAsync();
            }

            var factory = new GenericFilterFactory<ActivableTestModel>();
            var activableFilter = factory.GetFilters()[0];

            var qparams = new QueryParams<ActivableTestModel>() { Filter = e => e.Counter > 0 };
            activableFilter.UpdateParams(qparams);
            Assert.NotNull(qparams);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<ActivableTestModel>(context);
                var result = await service.FindAsync(qparams);
                Check(result);
            }                
        }

        [Fact]
        public async Task ActivableFilterTestsMongoDB()
        {
            var dbName = nameof(ActivableFilterTestsMongoDB);

            var context = GetMongoContext(dbName);            
            await context.GetCollection<ActivableTestModel>().InsertManyAsync(GetSeed<ActivableTestModel>());
            

            var factory = new GenericFilterFactory<ActivableTestModel>();
            var activableFilter = factory.GetFilters()[0];

            var qparams = new QueryParams<ActivableTestModel>() { Filter = e => e.Counter > 0 };
            activableFilter.UpdateParams(qparams);
            Assert.NotNull(qparams);
            
            var service = new MongoDbDataStorage<ActivableTestModel>(context);
            var result = await service.FindAsync(qparams);

            Check(result);

            _runner.Dispose();            
        }

       
    }
}
