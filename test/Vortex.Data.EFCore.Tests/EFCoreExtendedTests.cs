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
        private static ExtendedTestContext GetContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ExtendedTestContext>()
              .UseInMemoryDatabase(databaseName: databaseName)
              .Options;

            return new ExtendedTestContext(options);
        }

        private async Task SeedContext(string dbName)
        {
            using (var context = GetContext(dbName))
            {
                await context.ExtendedTestModels.AddAsync(
                    new ExtendedTestModel
                    {
                        Id = 1,
                        TestModelId = 1
                    }
                );

                await context.TestModels.AddAsync(
                    new TestModel {
                        Id = 1,
                        Text = "first entry",
                        Counter = 1,
                        Date = DateTime.Now,
                        TestModelFkId = 1
                    }
                );

                await context.TestModelFks.AddAsync(
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
        public async Task EFCoreExtendedTests_AddIncludes()
        {
            var dbName = nameof(EFCoreExtendedTests_AddIncludes);
            await SeedContext(dbName);

            using (var context = GetContext(dbName))
            {
                var result = context.TestModels.ToList();
                Assert.Null(result[0].TestModelFk);

                IQueryable<TestModel> includes = context.TestModels;
                result = await includes.AddIncludes(nameof(TestModelFk)).ToListAsync();
                Assert.NotNull(result[0].TestModelFk);
            }
        }

        [Fact]
        public async Task EFCoreExtendedTests_AddIncludesMultilevel()
        {
            var dbName = nameof(EFCoreExtendedTests_AddIncludesMultilevel);
            await SeedContext(dbName);

            using (var context = GetContext(dbName))
            {
                var result = context.ExtendedTestModels.ToList();
                Assert.Null(result[0].TestModel);

                IQueryable<ExtendedTestModel> includes = context.ExtendedTestModels;
                result = await includes.AddIncludes(
                    nameof(ExtendedTestModel.TestModel), 
                    $"{nameof(ExtendedTestModel.TestModel)}.{nameof(TestModel.TestModelFk)}").ToListAsync();
                Assert.NotNull(result[0].TestModel);
                Assert.NotNull(result[0].TestModel.TestModelFk);
            }
        }

        [Fact]
        public async Task EFCoreExtendedTests_InvalidProperty()
        {
            var dbName = nameof(EFCoreExtendedTests_InvalidProperty);
            await SeedContext(dbName);

            using (var context = GetContext(dbName))
            {
                IQueryable<TestModel> includes = context.TestModels;
                await Assert.ThrowsAsync<System.InvalidOperationException>(
                    async () => await includes.AddIncludes("somerandomproperty").ToListAsync());
            }
        }

        [Fact]
        public async Task EFCoreExtendedTests_AddIncludesNullParam()
        {
            var dbName = nameof(EFCoreExtendedTests_AddIncludesNullParam);
            await SeedContext(dbName);

            using (var context = GetContext(dbName))
            {
                IQueryable<TestModel> includes = context.TestModels;
                var result = await includes.AddIncludes(null).ToListAsync();
                Assert.Null(result[0].TestModelFk);
            }
        }
    }
}
