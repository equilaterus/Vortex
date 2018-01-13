using Equilaterus.Vortex.ModelActions;
using Equilaterus.Vortex.Models;
using Equilaterus.Vortex.Services.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Vortex.Tests.IntegrationTests
{
    public class ActivableFilterTests
    {
        public static TestContext GetContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TestContext>()
              .UseInMemoryDatabase(databaseName: databaseName)
              .Options;

            return new TestContext(options);
        }

        public class ActivableTestModel : IActivable
        {
            public int Id { get; set; }

            public bool IsActive { get; set; }

            public int Counter { get; set; }
        }

        public class TestContext : DbContext
        {
            public DbSet<ActivableTestModel> ActivableTestModels { get; set; }
            
            public TestContext()
            { }

            public TestContext(DbContextOptions<TestContext> options)
                : base(options)
            { }
        }

        [Fact]
        public async Task ActivableFilterTests_DoFilterEFCore()
        {            
            // Test against EF
            var dbName = nameof(ActivableFilterTests_DoFilterEFCore);

            using (var context = GetContext(dbName))
            {
                await context.AddRangeAsync(
                    new List<ActivableTestModel>() {
                        new ActivableTestModel { IsActive = true, Counter = 1},
                        new ActivableTestModel { IsActive = false, Counter = 1},
                        new ActivableTestModel { IsActive = true, Counter = 0},
                        new ActivableTestModel { IsActive = false, Counter = 0},
                        new ActivableTestModel { IsActive = true, Counter = 1}
                    }
                );

                await context.SaveChangesAsync();
            }

            var activableFilter = new ActivableFilter<ActivableTestModel>();

            var filter = activableFilter.Do(e => e.Counter > 0);

            Assert.NotNull(filter);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<ActivableTestModel>(context);
                var result = await service.FindAsync(filter);

                Assert.Equal(2, result.Count);
                foreach (var entity in result) {
                    Assert.True(entity.IsActive);
                    Assert.True(entity.Counter > 0);
                }
            }            
        }
    }
}
