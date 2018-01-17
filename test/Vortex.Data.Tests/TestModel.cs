using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Equilaterus.Vortex.Services.Tests
{
    public class BaseModel : IBaseModel<string>
    {
        public string Id { get; set; }

        public BaseModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class ModelA : BaseModel
    {
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int Counter { get; set; }

        public string ModelBId { get; set; }
        [ForeignKey("ModelBId")]
        public virtual ModelB ModelB { get; set; }

        public ModelA() : base() { }
    }

    public class ModelB : BaseModel
    {
        public string OtherText { get; set; }

        public ModelB() : base() { }
    }

    public static class TestModelHelpers
    {
        const string DEFAULT_TEXT = "random text";

        public static ModelA GetDefaultEntity(this DataStorageTests  ds)
        {
            return new ModelA { Text = DEFAULT_TEXT, Counter = 0, Date = DateTime.Now };
        }

        public static Tuple<List<ModelA>, List<ModelB>> GetSeed(this DataStorageTests ds)
        {
            var modelsB = new List<ModelB>() {
                new ModelB
                {
                    Id = "79b291a8-b3b6-45b5-8e40-922f92a79b47",
                    OtherText = "FK test"
                }
            };
            
            var modelsA = new List<ModelA>() {
                new ModelA
                {
                    Id = "f88f49d8-44c5-453d-969e-9ef6a2e5a8c9",
                    Text = "first entry",
                    Counter = 1,
                    Date = DateTime.Now,
                    ModelBId = modelsB[0].Id
                },
                new ModelA
                {
                    Id = "e2dd92a3-b492-4ba3-aef1-b6cc783ad5d0",
                    Text = "second entry",
                    Counter = 2,
                    Date = DateTime.Now,
                    ModelBId = modelsB[0].Id
                },
                new ModelA
                {
                    Id = "76f08c2f-3f6b-47e8-99f0-e97dcaf2e3a7",
                    Text = "third entry",
                    Counter = 3,
                    Date = DateTime.Now,
                    ModelBId = null
                },
                new ModelA
                {
                    Id = "c1633e72-e9d4-4a85-ba1e-74e34f923b28",
                    Text = "fourth entry",
                    Counter = 4,
                    Date = DateTime.Now,
                    ModelBId = null
                }
            };


            return Tuple.Create(modelsA, modelsB);
        }
    }
}
