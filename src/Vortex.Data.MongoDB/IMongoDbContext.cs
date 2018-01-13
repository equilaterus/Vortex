using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Services.MongoDB
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>();
    }
}
