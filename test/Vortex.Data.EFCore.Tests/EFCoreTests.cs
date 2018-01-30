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
    public class EFCoreTests : DataStorageTests
    {
        protected IDataStorage<ModelA> _service = null;

        private static TestContext GetContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TestContext>()
              .UseInMemoryDatabase(databaseName: databaseName)
              .Options;

            return new TestContext(options);
        }       

        protected override async Task<List<ModelA>> GetAllEntitiesAsync(string databaseName)
        {
            using (var context = GetContext(databaseName))
            {
                return await context.ModelsA.ToListAsync();
            }
        }

        protected override IDataStorage<ModelA> GetService(string databaseName)
        {
            return new EFCoreDataStorage<ModelA>(GetContext(databaseName));
        }

        protected override async Task SeedAsync(List<ModelA> entities, string databaseName)
        {
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(entities);
                await context.SaveChangesAsync();
            }
        }

        protected override void DisposeIfNecessary(IDataStorage<ModelA> service)
        {
            if (service is EFCoreDataStorage<ModelA> efCoreService)
            {
                efCoreService.DbContext.Dispose();
            }
        }
    }
}
