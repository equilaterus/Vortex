using Equilaterus.Vortex.Services.DataStorage.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services.EFCore.Tests
{
    public class TestContext : DbContext
    {
        public DbSet<ModelA> ModelsA { get; set; }

        public DbSet<ModelB> ModelsB { get; set; }

        public TestContext()
        { }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        { }
    }
}
