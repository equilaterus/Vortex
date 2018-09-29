using Equilaterus.Vortex.Saturn.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Equilaterus.Vortex.Saturn.Services.MongoDB
{
    public class MongoDbEntity : IBaseModel<string>
    {
        [BsonId]
        public string Id { get; set; }

        public MongoDbEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
