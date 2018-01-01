using Equilaterus.Vortex.Services.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.EFCore.Tests
{
    public class EFCoreExtendedTests
    {
        private TestContext GetContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TestContext>()
              .UseInMemoryDatabase(databaseName: databaseName)
              .Options;

            return new TestContext(options);
        }

        private async Task SeedContext(string dbName)
        {
            using (var context = GetContext(dbName))
            {
                await context.TestModels.AddRangeAsync(
                    new TestModel {
                        Id = 1,
                        Text = "first entry",
                        Counter = 1,
                        Date = DateTime.Now,
                        TestModelFkId = 1
                    }
                );

                await context.TestModelFks.AddRangeAsync(
                    new TestModelFk
                    {
                        Id = 1,
                        OtherText = "FK test"
                    }
                );

                await context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task EFCoreExtendedTests_ExtendedIncludes()
        {
            var dbName = nameof(EFCoreExtendedTests_ExtendedIncludes);
            await SeedContext(dbName);

            using (var context = GetContext(dbName))
            {
                var result = context.TestModels.ToList();
                Assert.True(result[0].TestModelFk == null);

                IQueryable<TestModel> includes = context.TestModels;
                result = includes.AddIncludes("TestModelFk").ToList();
                Assert.True(result[0].TestModelFk != null);
            }
        }        
    }
}
