using Equilaterus.Vortex.Services.DataStorage.Tests;
using Equilaterus.Vortex.Services.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.EFCore.Tests
{
    public class EFCoreTests : RelationalDataStorageTests<ModelA>
    {
        protected IDataStorage<ModelA> _service = null;

        protected TestContext GetContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TestContext>()
              .UseInMemoryDatabase(databaseName: databaseName)
              .Options;

            return new TestContext(options);
        }

        protected override void Check(List<ModelA> expected, List<ModelA> result)
        {
            //  Check size
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < result.Count; ++i)
            {
                var expectedEntity = expected[i];
                var resultEntity = result[i];

                // Check Ids
                Assert.Equal(expectedEntity.Id, resultEntity.Id);

                // Check other properties
                Assert.Equal(expectedEntity.Text, resultEntity.Text);
                Assert.Equal(expectedEntity.Date, resultEntity.Date);
                Assert.Equal(expectedEntity.Counter, resultEntity.Counter);
                Assert.Equal(expectedEntity.Value, resultEntity.Value);
                Assert.Equal(expectedEntity.FkId, resultEntity.FkId);
            }
        }

        protected override List<ModelA> GetSeedData()
        {
            var modelsA = new List<ModelA>() {
                new ModelA
                {
                    Id = "f88f49d8-44c5-453d-969e-9ef6a2e5a8c9",
                    Text = "first entry",
                    Counter = 1,
                    Date = DateTime.Now,
                    Value = 0.1f,
                    FkId = "004c289b-b41f-401d-8957-83c1b54f0093"
                },
                new ModelA
                {
                    Id = "e2dd92a3-b492-4ba3-aef1-b6cc783ad5d0",
                    Text = "second entry",
                    Counter = 2,
                    Date = DateTime.Now,
                    Value = 0.01f,
                    FkId = "004c289b-b41f-401d-8957-83c1b54f0093"
                },
                new ModelA
                {
                    Id = "76f08c2f-3f6b-47e8-99f0-e97dcaf2e3a7",
                    Text = "third entry",
                    Counter = 3,
                    Date = DateTime.Now,
                    Value = 0.001f
                },
                new ModelA
                {
                    Id = "c1633e72-e9d4-4a85-ba1e-74e34f923b28",
                    Text = "fourth entry",
                    Counter = 4,
                    Date = DateTime.Now,
                    Value = 0.00001f
                }
            };

            return modelsA;
        }
          

        protected override async Task<List<ModelA>> GetAllEntitiesAsync(string databaseName)
        {
            using (var context = GetContext(databaseName))
            {
                return await context.ModelsA.ToListAsync();
            }
        }

        protected override async Task SeedAsync(List<ModelA> entities, string databaseName)
        {
            using (var context = GetContext(databaseName))
            {
                await context.AddAsync(new ModelB() { Id = "004c289b-b41f-401d-8957-83c1b54f0093" });
                await context.AddRangeAsync(entities);
                await context.SaveChangesAsync();
            }
        }

        protected override void ClearOrDispose(IDataStorage<ModelA> service)
        {
            if (service is EFCoreDataStorage<ModelA> efCoreService)
            {
                efCoreService.DbContext.Dispose();
            }
        }

        protected override IDataStorage<ModelA> GetService(string databaseName)
        {
            return new EFCoreDataStorage<ModelA>(GetContext(databaseName));
        }

        protected override ModelA GetDefaultEntity()
        {
            return new ModelA { Text = "random text", Counter = 0, Date = DateTime.Now, Value = 0.1f };
        }
    }
}
