using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Equilaterus.Vortex.Services.EFCore.Tests
{
    #region Entities
    public class TestModel
    {
        public int Id { get; set; }

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
    #endregion

    #region Context
    public class TestContext : DbContext
    {
        public DbSet<TestModel> TestModels { get; set; }

        public DbSet<TestModelFk> TestModelFks { get; set; }

        public TestContext()
        { }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        { }
    }
    #endregion
}
