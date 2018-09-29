using Equilaterus.Vortex.Saturn.Services.Tests;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Equilaterus.Vortex.Saturn.Services.MongoDB.Tests
{
    public class TestModel : MongoDbEntity, IDataTestModel
    {
        public string Text { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonRepresentation(BsonType.String)]
        public DateTime Date { get; set; }

        public int Counter { get; set; }     

        public float Value { get; set; }

        public TestModel() : base() { }
    }    
}
