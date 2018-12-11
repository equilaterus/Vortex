using Equilaterus.Vortex.Filters;
using Equilaterus.Vortex.Saturn.Configuration;
using Equilaterus.Vortex.Saturn.Models;
using Equilaterus.Vortex.Saturn.Queries;
using Equilaterus.Vortex.Saturn.Queries.Filters;
using Equilaterus.Vortex.Saturn.Services;
using Equilaterus.Vortex.Saturn.Services.EFCore;
using Equilaterus.Vortex.Saturn.Services.MongoDB;
using Microsoft.EntityFrameworkCore;
using Mongo2Go;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.IntegrationTests
{
    public class ActivableTestModel : MongoDbEntity, IActivable
    {
        public bool IsActive { get; set; }
        public int Counter { get; set; }

        public ActivableTestModel() : base() { }
    }       

    public class ActivableFilterTests : IntegrationTest<ActivableTestModel>
    {
        protected override List<ActivableTestModel> GetSeed()
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

            using (var context = GetEfCoreContext(dbName))
            {
                await context.AddRangeAsync(GetSeed());
                await context.SaveChangesAsync();
            }

            var factory = new GenericFilterFactory();
            factory.LoadDefaults();

            var activableFilter = factory.GetFilters<ActivableTestModel>()[0];

            var qparams = new QueryParams<ActivableTestModel>() { Filter = e => e.Counter > 0 };
            activableFilter.UpdateParams(qparams);
            Assert.NotNull(qparams);

            using (var context = GetEfCoreContext(dbName))
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
            await context.GetCollection<ActivableTestModel>().InsertManyAsync(GetSeed());


            var factory = new GenericFilterFactory();
            factory.LoadDefaults();

            var activableFilter = factory.GetFilters<ActivableTestModel>()[0];

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
