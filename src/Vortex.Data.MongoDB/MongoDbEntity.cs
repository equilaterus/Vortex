using Equilaterus.Vortex.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Services.MongoDB
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
