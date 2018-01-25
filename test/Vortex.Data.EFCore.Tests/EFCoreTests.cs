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

        public static TestContext GetContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TestContext>()
              .UseInMemoryDatabase(databaseName: databaseName)
              .Options;

            return new TestContext(options);
        }        

        protected async Task Seed(string dbName)
        {
            await base.CreateSeed(
                new EFCoreDataStorage<ModelA>(GetContext(dbName)),
                new EFCoreDataStorage<ModelB>(GetContext(dbName)));
        }

        protected override async Task<IDataStorage<ModelA>> GetService(ContextType contextType)
        {
            var guid = Guid.NewGuid().ToString();
            if (contextType == ContextType.Seeded)
            {
                await Seed(guid);
            }
            return new EFCoreDataStorage<ModelA>(GetContext(guid));
            
        }        
    }
}
