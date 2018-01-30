using Equilaterus.Vortex.Services.DataStorage.Tests;
using Equilaterus.Vortex.Services.MongoDB;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.MongoDB.Tests
{
    public class MongoDbTests : DataStorageTests
    {
        static protected MongoDbRunner _runner;
        static protected MongoDbContext _context;

        protected MongoDbContext GetContext()
        {
            if (_context == null)
            {
                _runner = MongoDbRunner.Start();

                MongoDbSettings mongoDbSettings = new MongoDbSettings()
                {
                    ConnectionString = _runner.ConnectionString,
                    DatabaseName = "mongotest"
                };
                _context = new MongoDbContext(mongoDbSettings);
            }

            return _context;
        }

        protected override IDataStorage<ModelA> GetService(string databaseName)
        {
            return new MongoDbDataStorage<ModelA>(GetContext());
        }

        protected override async Task SeedAsync(List<ModelA> entities, string databaseName)
        {
            var context = GetContext();
            await context.GetCollection<ModelA>()
                .InsertManyAsync(entities);
        }

        protected override async Task<List<ModelA>> GetAllEntitiesAsync(string databaseName)
        {
            var context = GetContext();
            return await (await context.GetCollection<ModelA>().FindAsync(new BsonDocument())).ToListAsync();
        }

        protected override void DisposeIfNecessary(IDataStorage<ModelA> service)
        {
            _context.GetCollection<ModelA>().DeleteMany(e => e.Text != "");
        }
    }
}
