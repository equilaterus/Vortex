using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
/*
namespace Equilaterus.Vortex.Services.DataStorage.Tests
{
    public abstract class RelationalDataStorageTests
    {
        protected enum ContextType { Seeded, Empty };

        protected static void CheckListsAndIncludes(List<ModelA> expected, List<ModelA> result)
        {
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < result.Count; ++i)
            {
                Assert.Equal(expected[i].Id, result[i].Id);
                Assert.True(result[i].ModelB != null || result[i].ModelBId == null);
                if (result[i].ModelB != null)
                {
                    Assert.Equal(result[i].ModelBId, result[i].ModelB.Id);
                }
            }
        }

        protected static void CheckListsNullIncludes(List<ModelA> expected, List<ModelA> result)
        {
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < result.Count; ++i)
            {
                Assert.Equal(expected[i].Id, result[i].Id);
                Assert.Null(result[i].ModelB);
            }
        }

        protected abstract Task<IDataStorage<ModelA>> GetService(ContextType contextType);
        protected async Task CreateSeed(IDataStorage<ModelA> serviceA, IDataStorage<ModelB> serviceB)
        {
            var seed = this.GetSeed();
            await serviceB.InsertRangeAsync(seed.Item2);
            await serviceA.InsertRangeAsync(seed.Item1);

            CheckListsNullIncludes(seed.Item1, await serviceA.FindAllAsync());
        }

        [Fact]
        public async Task FindAll()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            var result = await service.FindAllAsync();
            var expected = this.GetSeed().Item1;

            CheckListsNullIncludes(expected, result);
        }


        [Fact]
        public async Task FindAllWithIncludes()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            var result = await service.FindAllAsync(
                   nameof(ModelA.ModelB));
            var expected = this.GetSeed().Item1;

            CheckListsAndIncludes(expected, result);
        }


        [Fact]
        public async Task FindDefaultParams()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            var result = await service.FindAsync();
            var expected = this.GetSeed().Item1;

            CheckListsNullIncludes(expected, result);
        }

        [Fact]
        public async Task FindDefaultParamsWithIncludes()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            var result = await service.FindAsync(
                    includeProperties: nameof(ModelA.ModelB));
            var expected = this.GetSeed().Item1;

            CheckListsAndIncludes(expected, result);
        }

        [Fact]
        public async Task FindFilter()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            var result = await service.FindAsync(
                   filter: e => e.ModelBId != null);
            var expected = this.GetSeed().Item1
                .Where(e => e.ModelBId != null).ToList();

            CheckListsNullIncludes(expected, result);
        }

        [Fact]
        public async Task FindSkipAndTake()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            var result = await service.FindAsync(
                skip: 1,
                take: 2);
            var expected = this.GetSeed().Item1
                .Skip(1)
                .Take(2).ToList();

            CheckListsNullIncludes(expected, result);
        }

        [Fact]
        public async Task FindSkipMaxThanAvailable()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            var result = await service.FindAsync(
                skip: 100);

            Assert.True(result.Count == 0);
        }

        [Fact]
        public async Task FindSkipNegative()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await service.FindAsync(skip: -1));
        }

        [Fact]
        public async Task FindTakeNegative()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await service.FindAsync(take: -1));
        }

        [Fact]
        public async Task FindTakeMaxThanAvailable()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            var result = await service.FindAsync(
                take: 100);
            var expected = this.GetSeed().Item1;

            Assert.Equal(expected.Count, result.Count);            
        }

        [Fact]
        public async Task FindOrderBy()
        {
            var service = await GetService(ContextType.Seeded);

            var result = await service.FindAsync(
                orderBy: e => e.OrderByDescending(t => t.Counter));

            var expected = this.GetSeed().Item1.OrderByDescending(t => t.Counter).ToList();

            CheckListsNullIncludes(expected, result);
        }

        [Fact]
        public async Task FindAllParams()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            var result = await service.FindAsync(
                filter: e => e.ModelB != null,
                skip: 1,
                take: 2,
                orderBy: e => e.OrderByDescending(t => t.Counter),
                includeProperties: nameof(ModelA.ModelB));

            var expected = this.GetSeed().Item1
                .Where(e => e.ModelBId != null)
                .OrderByDescending(t => t.Counter)
                .Skip(1)
                .Take(2).ToList();

            CheckListsAndIncludes(expected, result);

            var unordered = this.GetSeed().Item1
                .Where(e => e.ModelB != null)
                .Skip(1)
                .Take(2)
                .OrderByDescending(t => t.Counter).ToList();

            for (int i = 0; i < unordered.Count; ++i)
            {
                Assert.NotEqual(unordered[i].Id, result[i].Id);
            }            
        }

        [Fact]
        public async Task Insert()
        {
            var service = await GetService(ContextType.Empty);
            Assert.Empty(await service.FindAllAsync());

            var entity = this.GetDefaultEntity();

            var id = entity.Id;
            Assert.NotNull(entity.Id);
            await service.InsertAsync(
                entity
            );
            Assert.Equal(id, entity.Id);

            var result = await service.FindAllAsync();
            Assert.Single(result);
            Assert.Equal(entity.Id, result[0].Id);
            Assert.Equal(entity.Text, result[0].Text);
            Assert.Equal(entity.Counter, result[0].Counter);
            Assert.Equal(entity.Date, result[0].Date);
        }

        [Fact]
        public async Task InsertNull()
        {
            var service = await GetService(ContextType.Seeded);

            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await service.InsertAsync(null));            
        }

        [Fact]
        public async Task Update()
        {
            var service = await GetService(ContextType.Empty);
            Assert.Empty(await service.FindAllAsync());

            var entity = this.GetDefaultEntity();
            var id = entity.Id;
            await service.InsertRangeAsync(
                new List<ModelA>
                {
                    this.GetDefaultEntity(),
                    entity,
                    this.GetDefaultEntity()
                });

            Assert.Equal(3, (await service.FindAllAsync()).Count());

            var newEntity = new ModelA()
            {
                Counter = entity.Counter - 1,
                Text = entity.Text + "*",
                Id = id
            };

            await service.UpdateAsync(newEntity);   

            var result = (await service.FindAsync(
                filter: e => e.Id == id)).FirstOrDefault();

            Assert.Equal(id, result.Id);
            Assert.Equal(entity.Counter - 1, result.Counter);
            Assert.Equal(entity.Text + "*", result.Text);
        }

        [Fact]
        public async Task UpdateNull()
        {
            var service = await GetService(ContextType.Seeded);

            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await service.UpdateAsync(null));            
        }
        
        [Fact]
        public async Task Delete()
        {
            var service = await GetService(ContextType.Empty);
            Assert.Empty(await service.FindAllAsync());

            var entity = this.GetDefaultEntity();
            var id = entity.Id;
            await service.InsertRangeAsync(
                new List<ModelA>
                {
                    this.GetDefaultEntity(),
                    entity,
                    this.GetDefaultEntity()
                }
            );

            var newEntity = new ModelA { Id = id };
            await service.DeleteAsync(newEntity);

            var result = await service.FindAllAsync();
            Assert.Equal(2, result.Count);
            Assert.Null(result.Where(e => e.Id == id).FirstOrDefault());
        }

        [Fact]
        public async Task DeleteNull()
        {
            var service = await GetService(ContextType.Seeded);
            
            await Assert.ThrowsAsync<ArgumentNullException>(
                   async () => await service.DeleteAsync(null));            
        }

        [Fact]
        public async Task InsertRange()
        {
            var service = await GetService(ContextType.Empty);
            Assert.Empty(await service.FindAllAsync());

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

            await service.InsertRangeAsync(entities);

            var result = await service.FindAllAsync();
            for (int i = 0; i < result.Count; ++i)
            {
                Assert.Equal(ids[i], result[i].Id);
            }
        }

        [Fact]
        public async Task InsertRangeNull()
        {
            var service = await GetService(ContextType.Seeded);

            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await service.InsertRangeAsync(null));
        }

        [Fact]
        public async Task UpdateRange()
        {
            var service = await GetService(ContextType.Seeded);
            Assert.NotEmpty(await service.FindAllAsync());

            var entities = await service.FindAllAsync();

            Assert.True(entities.Sum(e => e.Counter) != 302);

            entities.ForEach(e => e.Counter = 0);
            entities[0].Counter = 302;

            await service.UpdateRangeAsync(entities);


            var result = await service.FindAllAsync();
            Assert.Equal(302, result.Sum(e => e.Counter));
           
        }

        [Fact]
        public async Task UpdateRangeNull()
        {
            var service = await GetService(ContextType.Seeded);

            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await service.UpdateRangeAsync(null));
            
        }

        [Fact]
        public async Task DeleteRange()
        {
            var service = await GetService(ContextType.Seeded);
            var entities = await service.FindAllAsync();
            Assert.NotEmpty(entities);

            entities.Remove(entities[0]);
            Assert.True(entities.Count > 1);
            
            await service.DeleteRangeAsync(entities);

            var result = await service.FindAllAsync();
            Assert.Single(result);            
        }

        [Fact]
        public async Task DeleteRangeNull()
        {
            var service = await GetService(ContextType.Seeded);

            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await service.DeleteRangeAsync(null));
            
        }
    }
}
*/