using Equilaterus.Vortex.Saturn.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests.Services
{
    public abstract class DocumentDataStorageTests<T> : DataStorageTests<T> where T  : class, IDataTestModel, new()
    {       
        [Fact]
        public async Task IncrementFieldOneEntity()
        {
            // Prepare
            var databaseName = nameof(IncrementFieldOneEntity);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);
                        
            var expected = seed; // Just for naming purpouses
            expected[0].Counter++;

            var entity = expected[0];

            // Execute
            var service = GetService(databaseName) as IDocumentDataStorage<T>;
            var result = await service.IncrementFieldAsync(e => e.Id == entity.Id, e => e.Counter, 1);

            // Check
            Check(new List<T> { entity }, new List<T> { result });
            Check(expected, await GetAllEntitiesAsync(databaseName));

            // End
            ClearOrDispose(service);
        }

        [Fact]
        public async Task IncrementFieldOneEntityTryingMultipleEntities()
        {
            // Prepare
            var databaseName = nameof(IncrementFieldOneEntityTryingMultipleEntities);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            var expected = seed; // Just for naming purpouses
            expected[0].Counter++;

            var entity = expected[0];

            // Execute
            var service = GetService(databaseName) as IDocumentDataStorage<T>;
            var result = await service.IncrementFieldAsync(e => e.Counter < 999, e => e.Counter, 1);

            // Check
            Check(new List<T> { entity }, new List<T> { result });
            Check(expected, await GetAllEntitiesAsync(databaseName));

            // End
            ClearOrDispose(service);
        }

        [Fact]
        public async Task IncrementFieldMultipleEntities()
        {
            // Prepare
            var databaseName = nameof(IncrementFieldMultipleEntities);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            var expected = seed; // Just for naming purpouses
            expected.ForEach(e => e.Counter += 1000);            

            // Execute
            var service = GetService(databaseName) as IDocumentDataStorage<T>;
            await service.IncrementFieldRangeAsync(e => e.Counter < 999, e => e.Counter, 1000);

            // Check
            Check(expected, await GetAllEntitiesAsync(databaseName));

            // End
            ClearOrDispose(service);
        }
    }
}
