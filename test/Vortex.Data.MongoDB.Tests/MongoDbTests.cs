using Equilaterus.Vortex.Services.MongoDB;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.MongoDB.Tests
{
    public class MongoDbTests
    {
        internal static MongoDbRunner _runner;
        internal static IMongoDbContext _context;
        internal static string _databaseName = "IntegrationTest";
        internal static string _collectionName = "TestCollection";

        const string DEFAULT_TEXT = "random text";
        public static TestModel GetDefaultEntity()
        {
            return new TestModel { Text = DEFAULT_TEXT, Counter = 0, Date = DateTime.Now };
        }

        internal static void CreateConnection()
        {
            _runner = MongoDbRunner.Start();

            MongoDbSettings mongoDbSettings = new MongoDbSettings()
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = _databaseName
            };

            _context = new MongoDbContext(mongoDbSettings);
        }

        static async Task Seed()
        {
            await _context.GetCollection<TestModel>().InsertManyAsync(
                new List<TestModel> {
                    GetDefaultEntity(), GetDefaultEntity()});
        }

        [Fact]
        public async Task MongoDb_FindAll()
        {
            CreateConnection();

            await Seed();

            MongoDbDataStorage<TestModel> service = new MongoDbDataStorage<TestModel>(_context);
            var result = await service.FindAllAsync();

            Assert.Equal(2, result.Count);
        }
    }
}
