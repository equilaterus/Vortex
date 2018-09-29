using MongoDB.Driver;

namespace Equilaterus.Vortex.Saturn.Services.MongoDB
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>();

        IMongoDatabase GetDatabase();
    }
}
