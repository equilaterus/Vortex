using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Equilaterus.Vortex.Saturn.Services.EFCore.Tests
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

    public class ExtendedTestModel
    {
        public int Id { get; set; }

        public int? TestModelId { get; set; }
        [ForeignKey("TestModelId")]
        public virtual TestModel TestModel { get; set; }
    }
    #endregion

    #region Context
    public class ExtendedTestContext : DbContext
    {
        public DbSet<TestModel> TestModels { get; set; }

        public DbSet<TestModelFk> TestModelFks { get; set; }

        public DbSet<ExtendedTestModel> ExtendedTestModels { get; set; }

        public ExtendedTestContext()
        { }

        public ExtendedTestContext(DbContextOptions<ExtendedTestContext> options)
            : base(options)
        { }
    }
    #endregion
}
