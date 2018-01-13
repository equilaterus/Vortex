using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Equilaterus.Vortex.Services.MongoDB
{
    public class MongoDbContext : IMongoDbContext
    {
        IMongoDatabase db;

        public IMongoCollection<T> GetCollection<T>()
        {
            return db.GetCollection<T>(typeof(T).Name);
        }
        
        public MongoDbContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            db = client.GetDatabase(settings.DatabaseName);
        }
    }
}
