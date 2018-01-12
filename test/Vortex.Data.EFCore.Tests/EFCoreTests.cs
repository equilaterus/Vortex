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
        public static TestModel GetDefaultEntity()
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
                    },
                    new TestModel
                    {
                        Id = 4,
                        Text = "fourth entry",
                        Counter = 4,
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
                var service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAllAsync();

                var expected = await context.TestModels.AsNoTracking().ToListAsync();

                Assert.Equal(expected.Count, result.Count);

                for (int i = 0; i < expected.Count; ++i)
                {
                    Assert.Equal(expected[i].Id, result[i].Id);
                    Assert.Null(result[i].TestModelFk);
                }
            }
        }

        [Fact]
        public async Task EFCoreTests_FindAllWithIncludes()
        {
            var dbName = nameof(EFCoreTests_FindAllWithIncludes);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAllAsync(
                    nameof(TestModel.TestModelFk));

                var expected = await context.TestModels.AsNoTracking().ToListAsync();

                Assert.Equal(expected.Count, result.Count);

                for (int i = 0; i < expected.Count; ++i)
                {
                    Assert.Equal(expected[i].Id, result[i].Id);
                    Assert.True(result[i].TestModelFk != null || result[i].TestModelFkId == null);
                }
            }
        }

        [Fact]
        public async Task EFCoreTests_FindDefaultParams()
        {
            var dbName = nameof(EFCoreTests_FindDefaultParams);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                EFCoreDataStorage<TestModel> service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAsync();

                var expected = await context.TestModels.AsNoTracking().ToListAsync();

                Assert.Equal(expected.Count, result.Count);

                for (int i = 0; i < expected.Count; ++i)
                {
                    Assert.Equal(expected[i].Id, result[i].Id);
                    Assert.Null(result[i].TestModelFk);
                }
            }
        }

        [Fact]
        public async Task EFCoreTests_FindDefaultParamsWithIncludes()
        {
            var dbName = nameof(EFCoreTests_FindDefaultParamsWithIncludes);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAsync(
                    includeProperties: nameof(TestModel.TestModelFk));

                var expected = await context.TestModels.AsNoTracking().ToListAsync();

                Assert.Equal(expected.Count, result.Count);

                for (int i = 0; i < expected.Count; ++i)
                {
                    Assert.Equal(expected[i].Id, result[i].Id);
                    Assert.True(result[i].TestModelFk != null || result[i].TestModelFkId == null);
                }
            }
        }

        [Fact]
        public async Task EFCoreTests_FindFilter()
        {
            var dbName = nameof(EFCoreTests_FindFilter);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAsync(
                    filter: e => e.TestModelFkId != null);

                var expected = await context.TestModels.AsNoTracking()
                    .Where(e => e.TestModelFkId != null).ToListAsync();

                Assert.Equal(expected.Count, result.Count);
                for (int i = 0; i < expected.Count; ++i)
                {                    
                    Assert.Equal(expected[i].Id, result[i].Id);
                    Assert.Null(result[i].TestModelFk);
                }
            }
        }

        [Fact]
        public async Task EFCoreTests_FindSkipAndTake()
        {
            var dbName = nameof(EFCoreTests_FindSkipAndTake);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAsync(
                    skip: 1,
                    take: 2);

                var expected = await context.TestModels.AsNoTracking().Skip(1).Take(2).ToListAsync();

                Assert.Equal(2, result.Count);
                for (int i = 0; i < expected.Count; ++i)
                {
                    Assert.Equal(expected[i].Id, result[i].Id);
                    Assert.Null(result[i].TestModelFk);
                }
            }
        }

        [Fact]
        public async Task EFCoreTests_FindSkipMaxThanAvailable()
        {
            var dbName = nameof(EFCoreTests_FindSkipMaxThanAvailable);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAsync(
                    skip: 100);                

                Assert.True(result.Count == 0);
            }
        }

        [Fact]
        public async Task EFCoreTests_FindSkipNegative()
        {
            var dbName = nameof(EFCoreTests_FindSkipNegative);
            
            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                    async () => await service.FindAsync(skip: -1));
            }
        }

        [Fact]
        public async Task EFCoreTests_FindTakeNegative()
        {
            var dbName = nameof(EFCoreTests_FindTakeNegative);
            
            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                    async () => await service.FindAsync(take: -1));
            }
        }

        [Fact]
        public async Task EFCoreTests_FindTakeMaxThanAvailable()
        {
            var dbName = nameof(EFCoreTests_FindTakeMaxThanAvailable);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAsync(
                    take: 100);

                var expected = await context.TestModels.AsNoTracking().ToListAsync();

                Assert.Equal(expected.Count, result.Count);
            }
        }

        [Fact]
        public async Task EFCoreTests_FindOrderBy()
        {
            var dbName = nameof(EFCoreTests_FindOrderBy);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAsync(
                    orderBy: e => e.OrderByDescending(t => t.Id));

                var expected = await context.TestModels.AsNoTracking().OrderByDescending(t => t.Id).ToListAsync();
                                
                for (int i = 0; i < expected.Count; ++i)
                {
                    Assert.Equal(expected[i].Id, result[i].Id);
                    Assert.Null(result[i].TestModelFk);
                }
            }
        }

        [Fact]
        public async Task EFCoreTests_FindAllParams()
        {
            var dbName = nameof(EFCoreTests_FindAllParams);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                var result = await service.FindAsync(
                    filter: e => e.TestModelFkId != null,
                    skip: 1,
                    take: 2,
                    orderBy: e => e.OrderByDescending(t => t.Id),
                    includeProperties: nameof(TestModel.TestModelFk));

                var expected = await context.TestModels.AsNoTracking()
                    .Where(e => e.TestModelFkId != null)
                    .OrderByDescending(t => t.Id)
                    .Skip(1)
                    .Take(2)                    
                    .Include(nameof(TestModel.TestModelFk)).ToListAsync();                

                for (int i = 0; i < expected.Count; ++i)
                {
                    Assert.Equal(expected[i].Id, result[i].Id);
                    Assert.NotNull(result[i].TestModelFk);
                }

                var unordered = await context.TestModels.AsNoTracking()
                    .Where(e => e.TestModelFkId != null)
                    .Skip(1)
                    .Take(2)
                    .OrderByDescending(t => t.Id)
                    .Include(nameof(TestModel.TestModelFk)).ToListAsync();

                for (int i = 0; i < unordered.Count; ++i)
                {
                    Assert.NotEqual(unordered[i].Id, result[i].Id);
                    Assert.NotNull(result[i].TestModelFk);
                }
            }
        }

        [Fact]
        public async Task EFCoreTests_Insert()
        {
            var dbName = nameof(EFCoreTests_Insert);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);
                await service.InsertAsync(
                    GetDefaultEntity()
                );
            }

            using (var context = GetContext(dbName))
            {
                Assert.Equal(1, await context.TestModels.AsNoTracking().CountAsync());
                Assert.Equal(DEFAULT_TEXT, (await context.TestModels.SingleOrDefaultAsync()).Text);
            }
        }

        [Fact]
        public async Task EFCoreTests_InsertNull()
        {
            var dbName = nameof(EFCoreTests_InsertNull);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () =>  await service.InsertAsync(null));
            }
        }

        [Fact]
        public async Task EFCoreTests_Update()
        {
            var dbName = nameof(EFCoreTests_Update);

            using (var context = GetContext(dbName))
            {
                await context.TestModels.AddAsync(GetDefaultEntity());
                await context.SaveChangesAsync();
            }

            using (var context = GetContext(dbName))
            {
                var entity = await context.TestModels.AsNoTracking().FirstOrDefaultAsync();

                entity.Counter = 1;
                entity.Text = DEFAULT_TEXT + "*";

                var service = new EFCoreDataStorage<TestModel>(context);
                await service.UpdateAsync(entity);
            }

            using (var context = GetContext(dbName))
            {
                var entity = await context.TestModels.FirstOrDefaultAsync();

                Assert.Equal(1, entity.Counter);
                Assert.NotEqual(DEFAULT_TEXT, entity.Text);
            }
        }

        [Fact]
        public async Task EFCoreTests_UpdateNull()
        {
            var dbName = nameof(EFCoreTests_UpdateNull);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await service.UpdateAsync(null));
            }
        }

        [Fact]
        public async Task EFCoreTests_Delete()
        {
            var dbName = nameof(EFCoreTests_Delete);

            using (var context = GetContext(dbName))
            {
                await context.TestModels.AddAsync(GetDefaultEntity());
                await context.SaveChangesAsync();
            }

            using (var context = GetContext(dbName))
            {
                var entity = await context.TestModels.AsNoTracking().FirstOrDefaultAsync();
               
                var service = new EFCoreDataStorage<TestModel>(context);
                await service.DeleteAsync(entity);
            }

            using (var context = GetContext(dbName))
            {
                Assert.Equal(0, await context.TestModels.AsNoTracking().CountAsync());
            }
        }

        [Fact]
        public async Task EFCoreTests_DeleteNull()
        {
            var dbName = nameof(EFCoreTests_DeleteNull);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await service.DeleteAsync(null));
            }
        }

        [Fact]
        public async Task EFCoreTests_InsertRange()
        {
            var dbName = nameof(EFCoreTests_InsertRange);
            
            using (var context = GetContext(dbName))
            {                
                var service = new EFCoreDataStorage<TestModel>(context);
                await service.InsertRangeAsync(
                    new List<TestModel>() { GetDefaultEntity(), GetDefaultEntity(), GetDefaultEntity() });
            }

            using (var context = GetContext(dbName))
            {
                Assert.Equal(3, await context.TestModels.CountAsync());
            }
        }

        [Fact]
        public async Task EFCoreTests_InsertRangeNull()
        {
            var dbName = nameof(EFCoreTests_InsertRangeNull);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await service.InsertRangeAsync(null));
            }
        }

        [Fact]
        public async Task EFCoreTests_UpdateRange()
        {
            var dbName = nameof(EFCoreTests_UpdateRange);
            await Seed(dbName);

            using (var context = GetContext(dbName))
            {
                var entities = await context.TestModels.AsNoTracking().ToListAsync();

                Assert.True(entities.Sum(e => e.Counter) != 302);
                                
                entities.ForEach(e => e.Counter = 0);
                entities[0].Counter = 302;

                var service = new EFCoreDataStorage<TestModel>(context);
                await service.UpdateRangeAsync(entities);
            }

            using (var context = GetContext(dbName))
            {
                Assert.Equal(302, await context.TestModels.AsNoTracking().SumAsync(e => e.Counter));
            }
        }

        [Fact]
        public async Task EFCoreTests_UpdateRangeNull()
        {
            var dbName = nameof(EFCoreTests_UpdateRangeNull);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await service.UpdateRangeAsync(null));
            }
        }

        [Fact]
        public async Task EFCoreTests_DeleteRange()
        {
            var dbName = nameof(EFCoreTests_DeleteRange);
            await Seed(dbName);
           
            using (var context = GetContext(dbName))
            {
                var entities = await context.TestModels.AsNoTracking().ToListAsync();
                entities.Remove(entities[0]);

                Assert.True(entities.Count > 0);

                var service = new EFCoreDataStorage<TestModel>(context);
                await service.DeleteRangeAsync(entities);
            }

            using (var context = GetContext(dbName))
            {
                Assert.Equal(1, await context.TestModels.CountAsync());                
            }
        }

        [Fact]
        public async Task EFCoreTests_DeleteRangeNull()
        {
            var dbName = nameof(EFCoreTests_DeleteRangeNull);

            using (var context = GetContext(dbName))
            {
                var service = new EFCoreDataStorage<TestModel>(context);

                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await service.DeleteRangeAsync(null));
            }
        }
    }
}
