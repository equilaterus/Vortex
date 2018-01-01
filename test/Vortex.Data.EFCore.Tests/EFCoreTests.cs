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
                EFCoreDataStorage<TestModel> service = new EFCoreDataStorage<TestModel>(context);

                var entity = await context.TestModels.FirstOrDefaultAsync();

                entity.Counter = 1;
                entity.Text = DEFAULT_TEXT + "*";

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
