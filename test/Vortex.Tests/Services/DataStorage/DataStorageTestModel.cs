using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Equilaterus.Vortex.Services.DataStorage.Tests
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

        public float Value { get; set; }
               
        public ModelA() : base() { }
    }
    
    public static class TestModelHelpers
    {
        const string DEFAULT_TEXT = "random text";

        public static ModelA GetDefaultEntity(this DataStorageTests  ds)
        {
            return new ModelA { Text = DEFAULT_TEXT, Counter = 0, Date = DateTime.Now, Value = 0.1f };
        }

        public static List<ModelA> GetSeedData(this DataStorageTests ds)
        {   
            var modelsA = new List<ModelA>() {
                new ModelA
                {
                    Id = "f88f49d8-44c5-453d-969e-9ef6a2e5a8c9",
                    Text = "first entry",
                    Counter = 1,
                    Date = DateTime.Now,
                    Value = 0.1f
                },
                new ModelA
                {
                    Id = "e2dd92a3-b492-4ba3-aef1-b6cc783ad5d0",
                    Text = "second entry",
                    Counter = 2,
                    Date = DateTime.Now,
                    Value = 0.01f
                },
                new ModelA
                {
                    Id = "76f08c2f-3f6b-47e8-99f0-e97dcaf2e3a7",
                    Text = "third entry",
                    Counter = 3,
                    Date = DateTime.Now,
                    Value = 0.001f
                },
                new ModelA
                {
                    Id = "c1633e72-e9d4-4a85-ba1e-74e34f923b28",
                    Text = "fourth entry",
                    Counter = 4,
                    Date = DateTime.Now,
                    Value = 0.00001f
                }
            };


            return modelsA;
        }
    }
}
