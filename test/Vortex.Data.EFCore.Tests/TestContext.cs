using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Services.EFCore.Tests
{
    public class TestContext : DbContext
    {
        public DbSet<TestModel> TestModels { get; set; }

        public TestContext()
        { }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        { }
    }
}
