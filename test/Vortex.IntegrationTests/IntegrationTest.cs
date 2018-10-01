using Equilaterus.Vortex.Saturn.Services.MongoDB;
using Microsoft.EntityFrameworkCore;
using Mongo2Go;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.IntegrationTests
{
    public abstract class IntegrationTest<T> where T : class, new()
    {        
        protected static TestContext<T> GetEfCoreContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TestContext<T>>()
              .UseInMemoryDatabase(databaseName: databaseName)
              .Options;

            return new TestContext<T>(options);
        }        

        protected class TestContext<TEntity> : DbContext where TEntity : class, T
        {
            public DbSet<TEntity> Models { get; set; }
                        
            public TestContext(DbContextOptions<TestContext<TEntity>> options)
                : base(options)
            { }
        }

        protected static MongoDbRunner _runner;
        protected static MongoDbContext GetMongoContext(string dbName)
        {
            _runner = MongoDbRunner.Start();
            MongoDbSettings mongoDbSettings = new MongoDbSettings()
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = dbName
            };
            return new MongoDbContext(mongoDbSettings);
        }

        protected abstract List<T> GetSeed();     
    }
}
