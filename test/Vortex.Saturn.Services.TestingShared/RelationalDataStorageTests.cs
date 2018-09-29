using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Services.Tests
{
    public interface IRelationalTestModel : IDataTestModel
    {
        string FkId { get; set; }

        IDataTestModel GetFkObject();

        string GetFkIncludeName();
    }

    public abstract class RelationalDataStorageTests<T> : DataStorageTests<T> where T  : class, IRelationalTestModel, new()
    {
        protected static void CheckIncludes(
            List<T> expected, 
            List<T> result, 
            bool hasIncludes = false)
        {
            bool validExpected = false;

            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < result.Count; ++i)
            {
                Assert.Equal(expected[i].Id, result[i].Id);
                if (!hasIncludes)
                {                    
                    Assert.Null(result[i].GetFkObject());
                    validExpected = true;
                }
                else
                {
                    Assert.True(result[i].GetFkObject() != null || result[i].FkId == null);
                    if (result[i].GetFkObject() != null)
                    {
                        Assert.Equal(result[i].FkId, result[i].GetFkObject().Id);
                        validExpected = true;
                    }
                }                
            }

            Assert.True(validExpected);
        }

        [Fact]
        public async Task FindAllWithoutIncludes()
        {
            // Prepare
            var databaseName = nameof(FindAllWithoutIncludes);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            // Execute
            var service = GetService(databaseName) as IRelationalDataStorage<T>;
            var result = await service.FindAllAsync();

            // Check
            Check(seed, result);
            CheckIncludes(seed, result, hasIncludes: false);

            // End
            ClearOrDispose(service);
        }

        [Fact]
        public async Task FindAllWithIncludes()
        {
            // Prepare
            var databaseName = nameof(FindAllWithIncludes);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            // Execute
            var service = GetService(databaseName) as IRelationalDataStorage<T>;
            var result = await service.FindAllAsync(new T().GetFkIncludeName());

            // Check
            Check(seed, result);
            CheckIncludes(seed, result, hasIncludes: true);

            // End
            ClearOrDispose(service);
        }

        
        [Fact]
        public async Task FindWithoutIncludes()
        {
            // Prepare
            var databaseName = nameof(FindWithoutIncludes);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            // Execute
            var service = GetService(databaseName) as IRelationalDataStorage<T>;
            var result = await service.FindAsync();

            // Check
            Check(seed, result);
            CheckIncludes(seed, result, hasIncludes: false);

            // End
            ClearOrDispose(service);
        }

        [Fact]
        public async Task FindWithIncludes()
        {
            // Prepare
            var databaseName = nameof(FindWithIncludes);
            var seed = GetSeedData();
            await SeedAsync(seed, databaseName);

            // Execute
            var service = GetService(databaseName) as IRelationalDataStorage<T>;
            var result = await service.FindAsync(includeProperties: new T().GetFkIncludeName());

            // Check
            Check(seed, result);
            CheckIncludes(seed, result, hasIncludes: true);

            // End
            ClearOrDispose(service);
        }


    }
}
