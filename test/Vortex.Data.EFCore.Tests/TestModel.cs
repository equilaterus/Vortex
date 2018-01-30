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
        
        public TestContext()
        { }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        { }
    }

    public class ModelA : ITestModel
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int Counter { get; set; }

        public float Value { get; set; }        

        public ModelA()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
