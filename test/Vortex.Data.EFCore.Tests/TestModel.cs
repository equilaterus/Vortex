using Equilaterus.Vortex.Models;
using Equilaterus.Vortex.Services.DataStorage.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services.EFCore.Tests
{
    public class TestContext : DbContext
    {
        public DbSet<ModelA> ModelsA { get; set; }
        public DbSet<ModelA> ModelsB { get; set; }

        public TestContext()
        { }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        { }
    }

    public class ModelA : IRelationalTestModel
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int Counter { get; set; }

        public float Value { get; set; }


        public string FkId { get; set; }

        [ForeignKey(nameof(FkId))]
        public virtual ModelB Fk { get; set; }

        public IDataTestModel GetFkObject() => Fk;

        public string GetFkIncludeName() => nameof(Fk);


        public ModelA()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class ModelB : IDataTestModel
    {
        public string Id { get; set; }
        public int Counter { get; set; }
    }
}
