using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Equilaterus.Vortex.Services.MongoDB
{
    public class MongoDbContext : IMongoDbContext
    {
        protected IMongoDatabase _database;

        public MongoDbContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public MongoDbContext(IOptions<MongoDbSettings> settings) : this(settings.Value)
        { }


        public IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }                
    }
}
