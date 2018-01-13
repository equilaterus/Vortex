using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Vortex.Data.MongoDB.Tests
{
    public class TestModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int Counter { get; set; }

        public int? TestModelFkId { get; set; }
        [ForeignKey("TestModelFkId")]
        public virtual TestModelFk TestModelFk { get; set; }
    }

    public class TestModelFk
    {
        public int Id { get; set; }

        public string OtherText { get; set; }
    }
}
