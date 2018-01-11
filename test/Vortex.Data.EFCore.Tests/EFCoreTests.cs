using Equilaterus.Vortex.Services.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.EFCore.Tests
{
    public class EFCoreTests
    {
        public static TestContext GetContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TestContext>()
              .UseInMemoryDatabase(databaseName: databaseName)
              .Options;

            return new TestContext(options);
        }

        const string DEFAULT_TEXT = "random text";
        public static TestModel GetDefafultEntity()
        {
            return new TestModel { Text = DEFAULT_TEXT, Counter = 0, Date = DateTime.Now };
        }

        private static async Task Seed(string dbName)
        {
            using (var context = GetContext(dbName))
            {
                await context.TestModels.AddRangeAsync(
                    new TestModel
                    {
                        Id = 1,
                        Text = "first entry",
                        Counter = 1,
                        Date = DateTime.Now,
                        TestModelFkId = 1
                    },
                    new TestModel
                    {
                        Id = 2,
                        Text = "second entry",
                        Counter = 2,
                        Date = DateTime.Now,
                        TestModelFkId = 1
                    },
                    new TestModel
                    {
                        Id = 3,
                        Text = "third entry",
                        Counter = 3,
                        Date = DateTime.Now,
                        TestModelFkId = null
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
        public async Task EFCoreTests_FindAll()
        {
            var dbName = nameof(EFCoreTests_FindAll);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                EFCoreDataStorage<TestModel> service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAllAsync();
                Assert.Equal(await context.TestModels.CountAsync(), result.Count());
                Assert.Null(result[0].TestModelFk);
            }
        }

        [Fact]
        public async Task EFCoreTests_FindAllWithIncludes()
        {
            var dbName = nameof(EFCoreTests_FindAllWithIncludes);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                EFCoreDataStorage<TestModel> service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAllAsync(nameof(TestModel.TestModelFk));
                Assert.Equal(await context.TestModels.CountAsync(), (await service.FindAllAsync()).Count());
                Assert.NotNull(result[0].TestModelFk);
            }
        }

        [Fact]
        public async Task EFCoreTests_InsertOneEntity()
        {
            var dbName = nameof(EFCoreTests_InsertOneEntity);

            using (var context = GetContext(dbName))
            {
                EFCoreDataStorage<TestModel> service = new EFCoreDataStorage<TestModel>(context);
                await service.InsertAsync(
                    GetDefafultEntity()
                );
            }

            using (var context = GetContext(dbName))
            {
                Assert.Equal(1, await context.TestModels.CountAsync());
                Assert.Equal(DEFAULT_TEXT, (await context.TestModels.SingleOrDefaultAsync()).Text);
            }
        }

        [Fact]
        public async Task EFCoreTests_UpdateOneEntity()
        {
            var dbName = nameof(EFCoreTests_UpdateOneEntity);

            using (var context = GetContext(dbName))
            {
                await context.TestModels.AddAsync(GetDefafultEntity());
                await context.SaveChangesAsync();
            }

            using (var context = GetContext(dbName))
            {
                var entity = await context.TestModels.FirstOrDefaultAsync();

                entity.Counter = 1;
                entity.Text = DEFAULT_TEXT + "*";

                EFCoreDataStorage<TestModel> service = new EFCoreDataStorage<TestModel>(context);
                await service.UpdateAsync(
                    entity
                );
            }

            using (var context = GetContext(dbName))
            {
                var entity = await context.TestModels.FirstOrDefaultAsync();

                Assert.Equal(1, entity.Counter);
                Assert.NotEqual(DEFAULT_TEXT, entity.Text);
            }
        }
    }
}
