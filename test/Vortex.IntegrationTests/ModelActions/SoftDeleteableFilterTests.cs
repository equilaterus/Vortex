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
    public class SoftDeleteableTestModel : MongoDbEntity, ISoftDeleteable
    {
        public bool IsDeleted { get; set; }
        public int Counter { get; set; }

        public SoftDeleteableTestModel() : base() { }
    }       

    public class SoftDeleteableFilterTests : IntegrationTest<SoftDeleteableTestModel>
    {
        protected override List<SoftDeleteableTestModel> GetSeed()
        {
            return new List<SoftDeleteableTestModel>()
                {
                    new SoftDeleteableTestModel { IsDeleted = true, Counter = 1},
                    new SoftDeleteableTestModel { IsDeleted = false, Counter = 1},
                    new SoftDeleteableTestModel { IsDeleted = true, Counter = 0},
                    new SoftDeleteableTestModel { IsDeleted = false, Counter = 0},
                    new SoftDeleteableTestModel { IsDeleted = false, Counter = 1}
                };
        }

        private void Check(List<SoftDeleteableTestModel> result)
        {
            Assert.Equal(2, result.Count);
            foreach (var entity in result)
            {
                Assert.False(entity.IsDeleted);
                Assert.True(entity.Counter > 0);
            }
        }

        [Fact]
        public async Task SoftDeleteableFilterTestsEFCore()
        {            
            var dbName = nameof(SoftDeleteableFilterTestsEFCore);

            using (var context = GetEfCoreContext(dbName))
            {
                await context.AddRangeAsync(GetSeed());
                await context.SaveChangesAsync();
            }

            var factory = new GenericFilterFactory();
            factory.LoadDefaults();

            var activableFilter = factory.GetFilters<SoftDeleteableTestModel>()[0];

            var qparams = new QueryParams<SoftDeleteableTestModel>() { Filter = e => e.Counter > 0 };
            activableFilter.UpdateParams(qparams);
            Assert.NotNull(qparams);

            using (var context = GetEfCoreContext(dbName))
            {
                var service = new EFCoreDataStorage<SoftDeleteableTestModel>(context);
                var result = await service.FindAsync(qparams);
                Check(result);
            }                
        }

        [Fact]
        public async Task SoftDeleteableTestsMongoDB()
        {
            var dbName = nameof(SoftDeleteableTestsMongoDB);

            var context = GetMongoContext(dbName);            
            await context.GetCollection<SoftDeleteableTestModel>().InsertManyAsync(GetSeed());

            var factory = new GenericFilterFactory();
            factory.LoadDefaults();

            var activableFilter = factory.GetFilters<SoftDeleteableTestModel>()[0];

            var qparams = new QueryParams<SoftDeleteableTestModel>() { Filter = e => e.Counter > 0 };
            activableFilter.UpdateParams(qparams);
            Assert.NotNull(qparams);
            
            var service = new MongoDbDataStorage<SoftDeleteableTestModel>(context);
            var result = await service.FindAsync(qparams);

            Check(result);

            _runner.Dispose();            
        }

       
    }
}
