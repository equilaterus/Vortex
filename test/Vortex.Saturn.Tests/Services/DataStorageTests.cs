using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.DataStorage.Tests
{
    public interface IDataTestModel
    {
        string Id { get; set; }
        int Counter { get; set; }
    }

    /// <summary>
    /// Base class for data storage tests.
    /// </summary>
    public abstract class DataStorageTests<T> where T : class, IDataTestModel, new()
    {
        /// <summary>
        /// You must check if the given lists have the same information
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="result"></param>
        protected abstract void Check(List<T> expected, List<T> result);

        /// <summary>
        /// You must return the default data seed
        /// </summary>
        /// <returns>A list with at least 4 elements</returns>
        protected abstract List<T> GetSeedData();
        
        /// <summary>
        /// You must return a service with a new context for the provided databaseName
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        protected abstract IDataStorage<T> GetService(string databaseName);

        /// <summary>
        /// You must return a default entity
        /// </summary>
        /// <returns></returns>
        protected abstract T GetDefaultEntity();

        /// <summary>
        /// You must insert the entities into the provided databaseName directly using your driver provider
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        protected abstract Task SeedAsync(List<T> entities, string databaseName);

        /// <summary>
        /// Return the content of the database of name databaseName directly using your driver provider.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        protected abstract Task<List<T>> GetAllEntitiesAsync(string databaseName);

        /// <summary>
        /// Dispose if necessary unmanaged resources like DbContext.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        protected abstract void ClearOrDispose(IDataStorage<T> service);
        
        [Fact]
        public async Task EnsureSeed()
        {
            // Prepare
            var databaseName = nameof(EnsureSeed);
            var service = GetService(databaseName);

            var seed = GetSeedData();
            Assert.True(seed.Count() >= 4);

            // Execute
            await SeedAsync(seed, databaseName);

            // Check            
            Check(seed, await GetAllEntitiesAsync(databaseName));

            // End
            ClearOrDispose(service);
        }

        [Fact]
        public async Task FindAll()
        {
            // Prepare
            var databaseName = nameof(FindAll);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAllAsync();

            // Check
            Check(seed, result);

            // End
            ClearOrDispose(service);
        }       


        [Fact]
        public async Task FindDefaultParams()
        {
            // Prepare
            var databaseName = nameof(FindDefaultParams);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAsync();            

            // Check
            Check(seed, result);

            // End
            ClearOrDispose(service);
        }
                
        [Fact]
        public async Task FindFilter()
        {
            // Prepare
            var databaseName = nameof(FindFilter);
            var seed = GetSeedData();
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
            ClearOrDispose(service);
        }

        [Fact]
        public async Task FindSkipAndTake()
        {
            // Prepare
            var databaseName = nameof(FindSkipAndTake);
            var seed = GetSeedData();
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
            ClearOrDispose(service);
        }

        [Fact]
        public async Task FindSkipMaxThanAvailable()
        {
            // Prepare
            var databaseName = nameof(FindSkipMaxThanAvailable);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAsync(
                skip: 100);

            Assert.True(result.Count == 0);

            // End
            ClearOrDispose(service);
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
            ClearOrDispose(service);
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
            ClearOrDispose(service);
        }

        [Fact]
        public async Task FindTakeMaxThanAvailable()
        {
            // Prepare
            var databaseName = nameof(FindTakeMaxThanAvailable);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);
            
            // Execute
            var service = GetService(databaseName);
            var result = await service.FindAsync(
                take: 100);

            // Check
            Check(seed, result);

            // End
            ClearOrDispose(service);
        }

        [Fact]
        public async Task FindOrderBy()
        {
            // Prepare
            var databaseName = nameof(FindOrderBy);
            var seed = GetSeedData();
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
            ClearOrDispose(service);
        }

        [Fact]
        public async Task FindAllParams()
        {
            // Prepare
            var databaseName = nameof(FindAllParams);
            var seed = GetSeedData();
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
            ClearOrDispose(service);
        }

        [Fact]
        public async Task CountAll()
        {
            // Prepare
            var databaseName = nameof(CountAll);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            var expected = seed.Count();

            // Execute
            var service = GetService(databaseName);
            var result = await service.Count();

            // Check
            Assert.Equal(expected, result);

            // End
            ClearOrDispose(service);
        }

        [Fact]
        public async Task CountFilter()
        {
            // Prepare
            var databaseName = nameof(CountFilter);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            var expected = seed.
                Where(e => e.Counter > 1).ToList();
            Assert.NotEqual(seed.Count, expected.Count);

            // Execute
            var service = GetService(databaseName);
            var result = await service.Count(
                   filter: e => e.Counter > 1);

            // Check
            Assert.Equal(expected.Count, result);

            // End
            ClearOrDispose(service);
        }

        [Fact]
        public async Task Insert()
        {
            // Prepare
            var databaseName = nameof(Insert);

            var entity = GetDefaultEntity();
            var expected = new List<T>() { entity };

            var id = entity.Id;

            // Execute
            var service = GetService(databaseName);
            await service.InsertAsync(
                entity
            );
            var result = await GetAllEntitiesAsync(databaseName);

            // Check
            Assert.Single(result);
            Check(expected, result);

            // It must preserve the id
            Assert.Equal(id, entity.Id);
            Assert.Equal(id, result[0].Id);

            // End
            ClearOrDispose(service);
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
            ClearOrDispose(service);
        }

        [Fact]
        public async Task Update()
        {
            // Prepare
            var databaseName = nameof(Update);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            var originalEntity = seed[0];
            var updatedEntity = new T()
            {
                Id = originalEntity.Id,
                Counter = originalEntity.Counter - 1               
            };

            var expected = new List<T>()
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
            ClearOrDispose(service);
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
            ClearOrDispose(service);
        }
        
        [Fact]
        public async Task Delete()
        {
            // Prepare
            var databaseName = nameof(Delete);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            var entityToDelete = new T { Id = seed[0].Id };

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
            ClearOrDispose(service);
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
            ClearOrDispose(service);
        }

        [Fact]
        public async Task InsertRange()
        {
            // Prepare
            var databaseName = nameof(InsertRange);            

            var entities = new List<T>() {
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
            ClearOrDispose(service);
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
            ClearOrDispose(service);
        }

        [Fact]
        public async Task UpdateRange()
        {
            // Prepare
            var databaseName = nameof(UpdateRange);
            var seed = GetSeedData();
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
            ClearOrDispose(service);
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
            ClearOrDispose(service);
        }

        [Fact]
        public async Task DeleteRange()
        {
            // Prepare
            var databaseName = nameof(DeleteRange);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            var expected = new List<T>() { seed[0] };

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
            ClearOrDispose(service);
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
            ClearOrDispose(service);
        }
    }
}
