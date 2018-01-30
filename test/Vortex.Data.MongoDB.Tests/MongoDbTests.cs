using Equilaterus.Vortex.Services.DataStorage.Tests;
using Equilaterus.Vortex.Services.MongoDB;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.MongoDB.Tests
{    
    public class MongoDbTests : DataStorageTests<TestModel>
    {
        protected  MongoDbRunner _runner;
        protected MongoDbContext _context;

        protected MongoDbContext GetContext(string databaseName)
        {             
            if (_context == null)
            {
                _runner = MongoDbRunner.Start();
                MongoDbSettings mongoDbSettings = new MongoDbSettings()
                {
                    ConnectionString = _runner.ConnectionString,
                    DatabaseName = databaseName
                };
                _context = new MongoDbContext(mongoDbSettings);
            }

            return _context;
        }

        protected override void Check(List<TestModel> expected, List<TestModel> result)
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

        protected override IDataStorage<TestModel> GetService(string databaseName)
        {
            return new MongoDbDataStorage<TestModel>(GetContext(databaseName));
        }

        protected override async Task SeedAsync(List<TestModel> entities, string databaseName)
        {
            var context = GetContext(databaseName);
            await context.GetCollection<TestModel>()
                .InsertManyAsync(entities);
        }

        protected override async Task<List<TestModel>> GetAllEntitiesAsync(string databaseName)
        {
            var context = GetContext(databaseName);
            return await (await context.GetCollection<TestModel>().FindAsync(new BsonDocument())).ToListAsync();
        }

        protected override void ClearOrDispose(IDataStorage<TestModel> service)
        {
            _runner.Dispose();
        }        

        protected override List<TestModel> GetSeedData()
        {
            var entities = new List<TestModel>() {
                new TestModel
                {
                    Id = "f88f49d8-44c5-453d-969e-9ef6a2e5a8c9",
                    Text = "first entry",
                    Counter = 1,
                    Date = DateTime.Now,
                    Value = 0.1f
                },
                new TestModel
                {
                    Id = "e2dd92a3-b492-4ba3-aef1-b6cc783ad5d0",
                    Text = "second entry",
                    Counter = 2,
                    Date = DateTime.Now,
                    Value = 0.01f
                },
                new TestModel
                {
                    Id = "76f08c2f-3f6b-47e8-99f0-e97dcaf2e3a7",
                    Text = "third entry",
                    Counter = 3,
                    Date = DateTime.Now,
                    Value = 0.001f
                },
                new TestModel
                {
                    Id = "c1633e72-e9d4-4a85-ba1e-74e34f923b28",
                    Text = "fourth entry",
                    Counter = 4,
                    Date = DateTime.Now,
                    Value = 0.00001f
                }
            };

            return entities;
        }

        protected override TestModel GetDefaultEntity()
        {
            return new TestModel { Text = "random text", Counter = 0, Date = DateTime.Now, Value = 0.1f };
        }        
    }
}
