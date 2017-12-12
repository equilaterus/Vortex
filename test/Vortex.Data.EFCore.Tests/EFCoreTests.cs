using Equilaterus.Vortex.Services.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.EFCore.Tests
{
    public class EFCoreTests
    {
        private TestContext GetContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TestContext>()
              .UseInMemoryDatabase(databaseName: databaseName)
              .Options;

            return new TestContext(options);
        }

        [Fact]
        public async Task EFCoreTests_InsertOneEntity()
        {
            var db = "InsertOne";
            using (var context = GetContext(db))
            {
                EFCoreDataStorage<TestModel> service = new EFCoreDataStorage<TestModel>(context);
                await service.InsertAsync(
                    new TestModel { Text = "random text", Counter = 0, Date = DateTime.Now }
                );
            }
            
            using (var context = GetContext(db))
            {
                Assert.Equal(1, await context.TestModels.CountAsync());
                Assert.Equal("random text", (await context.TestModels.SingleOrDefaultAsync()).Text);
            }
        }
    }
}
