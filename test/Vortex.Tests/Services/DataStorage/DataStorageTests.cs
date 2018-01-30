using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.DataStorage.Tests
{
    /// <summary>
    /// Base class for data storage tests.
    /// </summary>
    public abstract class DataStorageTests
    {
        protected enum ContextType { Seeded, Empty };
        
        protected static void Check(List<ModelA> expected, List<ModelA> result)
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
            }
        }
        
        /// <summary>
        /// You must return a service with a new context for the provided databaseName
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        protected abstract IDataStorage<ModelA> GetService(string databaseName);

        /// <summary>
        /// You must insert the entities into the provided databaseName directly using your driver provider
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        protected abstract Task SeedAsync(List<ModelA> entities, string databaseName);

        /// <summary>
        /// Return the content of the database of name databaseName directly using your driver provider.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        protected abstract Task<List<ModelA>> GetAllEntitiesAsync(string databaseName);

        /// <summary>
        /// Dispose if necessary unmanaged resources like DbContext.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        protected abstract void DisposeIfNecessary(IDataStorage<ModelA> service);
        
        [Fact]
        public async Task EnsureSeed()
        {
            // Prepare
            var databaseName = nameof(EnsureSeed);
            var service = GetService(databaseName);

            var seed = this.GetSeedData();
            Assert.Equal(4, seed.Count());

            // Execute
            await SeedAsync(seed, databaseName);

            // Check            
            Check(seed, await GetAllEntitiesAsync(databaseName));

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task FindAll()
        {
            // Prepare
            var databaseName = nameof(FindAll);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);

            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAllAsync();

            // Check
            Check(seed, result);

            // End
            DisposeIfNecessary(service);
        }       


        [Fact]
        public async Task FindDefaultParams()
        {
            // Prepare
            var databaseName = nameof(FindDefaultParams);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);

            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAsync();            

            // Check
            Check(seed, result);

            // End
            DisposeIfNecessary(service);
        }
                
        [Fact]
        public async Task FindFilter()
        {
            // Prepare
            var databaseName = nameof(FindFilter);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);

            var expected = seed.
                Where(e => e.Counter > 1).ToList();
            Assert.NotEqual(seed.Count, expected.Count);

            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAsync(
                   filter: e => e.Counter > 1);            

            // Check
            Check(expected, result);

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task FindSkipAndTake()
        {
            // Prepare
            var databaseName = nameof(FindSkipAndTake);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);

            var expected = seed
                .Skip(1)
                .Take(2).ToList();
            Assert.NotEqual(seed.Count, expected.Count);

            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAsync(
                skip: 1,
                take: 2);
            
            // Check
            Check(expected, result);

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task FindSkipMaxThanAvailable()
        {
            // Prepare
            var databaseName = nameof(FindSkipMaxThanAvailable);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);

            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAsync(
                skip: 100);

            Assert.True(result.Count == 0);

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task FindSkipNegative()
        {
            // Prepare
            var databaseName = nameof(FindSkipNegative);

            // Execute and check
            var service = GetService(databaseName);
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await service.FindAsync(skip: -1));

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task FindTakeNegative()
        {
            // Prepare
            var databaseName = nameof(FindSkipNegative);

            // Execute
            var service = GetService(databaseName);

            // Check
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await service.FindAsync(take: -1));

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task FindTakeMaxThanAvailable()
        {
            // Prepare
            var databaseName = nameof(FindTakeMaxThanAvailable);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);
            
            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAsync(
                take: 100);

            // Check
            Check(seed, result);

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task FindOrderBy()
        {
            // Prepare
            var databaseName = nameof(FindOrderBy);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);

            var expected = seed
                .OrderByDescending(t => t.Counter).ToList();
            Assert.NotEqual(seed[0].Counter, expected[0].Counter);

            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAsync(
               orderBy: e => e.OrderByDescending(t => t.Counter));

            // Check
            Check(expected, result);

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task FindAllParams()
        {
            // Prepare
            var databaseName = nameof(FindAllParams);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);

            var expected = seed
                .Where(e => e.Counter > 1)
                .OrderByDescending(t => t.Counter)
                .Skip(1)
                .Take(2).ToList();
            Assert.Equal(3, expected[0].Counter);
            Assert.Equal(2, expected[1].Counter);

            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAsync(
                filter: e => e.Counter > 1,
                orderBy: e => e.OrderByDescending(t => t.Counter),
                skip: 1,
                take: 2);            

            // Check
            Check(expected, result);

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task Insert()
        {
            // Prepare
            var databaseName = nameof(Insert);

            var entity = this.GetDefaultEntity();
            var expected = new List<ModelA>() { entity };

            var id = entity.Id;
            Assert.NotNull(entity.Id);

            // Execute
            var service = GetService(databaseName);
            await service.InsertAsync(
                entity
            );
            var result = await GetAllEntitiesAsync(databaseName);

            // Check
            Assert.Single(result);
            Check(expected, result);

            // Original entity must have the beginning Id
            Assert.Equal(id, entity.Id);

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task InsertNull()
        {
            // Prepare
            var databaseName = nameof(InsertNull);

            // Execute and check
            var service = GetService(databaseName);
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await service.InsertAsync(null));

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task Update()
        {
            // Prepare
            var databaseName = nameof(Update);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);

            var originalEntity = seed[0];
            var updatedEntity = new ModelA()
            {
                Counter = originalEntity.Counter - 1,
                Text = originalEntity.Text + "*",
                Date = originalEntity.Date.AddYears(1),
                Value = originalEntity.Value + 0.1f,
                Id = originalEntity.Id
            };

            var expected = new List<ModelA>()
            {
                updatedEntity,
                seed[1],
                seed[2],
                seed[3]
            };


            // Execute
            var service = GetService(databaseName);
            await service.UpdateAsync(updatedEntity);   

            // Check
            var result = await GetAllEntitiesAsync(databaseName);
            Check(expected, result);

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task UpdateNull()
        {
            // Prepare
            var databaseName = nameof(UpdateNull);

            // Execute and check
            var service = GetService(databaseName);
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await service.UpdateAsync(null));

            // End
            DisposeIfNecessary(service);
        }
        
        [Fact]
        public async Task Delete()
        {
            // Prepare
            var databaseName = nameof(Delete);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);

            var entityToDelete = new ModelA { Id = seed[0].Id };

            var expected = seed;
            expected.RemoveAt(0);

            // Execute
            var service = GetService(databaseName);
            await service.DeleteAsync(entityToDelete);

            // Check
            var result = await GetAllEntitiesAsync(databaseName);

            Assert.Null(result.Where(e => e.Id == entityToDelete.Id).FirstOrDefault());
            Check(expected, result);

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task DeleteNull()
        {
            // Prepare
            var databaseName = nameof(DeleteNull);

            // Execute and check
            var service = GetService(databaseName);            
            await Assert.ThrowsAsync<ArgumentNullException>(
                   async () => await service.DeleteAsync(null));

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task InsertRange()
        {
            // Prepare
            var databaseName = nameof(InsertRange);            

            var entities = new List<ModelA>() {
                this.GetDefaultEntity(),
                this.GetDefaultEntity(),
                this.GetDefaultEntity()
            };

            var ids = new List<string>()
            {
                entities[0].Id,
                entities[1].Id,
                entities[2].Id
            };

            // Execute
            var service = GetService(databaseName);
            await service.InsertRangeAsync(entities);

            // Check
            Check(entities, await GetAllEntitiesAsync(databaseName));            

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task InsertRangeNull()
        {
            // Prepare
            var databaseName = nameof(InsertRangeNull);

            // Execute and check
            var service = GetService(databaseName);
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await service.InsertRangeAsync(null));

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task UpdateRange()
        {
            // Prepare
            var databaseName = nameof(UpdateRange);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);
            
            var entities = seed; // Only for naming purpouses
            entities.ForEach(e => e.Counter = 0);
            entities[0].Counter = 0451;


            // Execute
            var service = GetService(databaseName);
            await service.UpdateRangeAsync(entities);

            // Check
            Check(entities, await GetAllEntitiesAsync(databaseName));

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task UpdateRangeNull()
        {
            // Prepare
            var databaseName = nameof(UpdateRangeNull);

            // Execute and check
            var service = GetService(databaseName);
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await service.UpdateRangeAsync(null));

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task DeleteRange()
        {
            // Prepare
            var databaseName = nameof(DeleteRange);
            var seed = this.GetSeedData();
            await SeedAsync(seed, databaseName);

            var expected = new List<ModelA>() { seed[0] };

            var entities = seed; // Only for naming purpouses
            seed.RemoveAt(0);
            Assert.True(seed.Count > 1);

            // Execute
            var service = GetService(databaseName);
            await service.DeleteRangeAsync(entities);

            var result = await GetAllEntitiesAsync(databaseName);

            // Check
            Assert.Single(result);
            Check(expected, result);

            // End
            DisposeIfNecessary(service);
        }

        [Fact]
        public async Task DeleteRangeNull()
        {
            // Prepare
            var databaseName = nameof(DeleteRangeNull);

            // Execute and check
            var service = GetService(databaseName);
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await service.DeleteRangeAsync(null));

            // End
            DisposeIfNecessary(service);
        }
    }
}
