
using Equilaterus.Vortex.Saturn.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Saturn.Tests
{
    public class TestModel
    {
        public int Id { get; set; }
    }

    public class AdjuntableTestModel : IAttacheableFile
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }

        public AdjuntableTestModel(string fileUrl)
        {
            FileUrl = fileUrl;
        }

        public AdjuntableTestModel() { }
    }

    public class ActivableTestModel : IActivable
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }
    }

    public class SoftDeleteableTestModel : ISoftDeleteable
    {
        public int Id { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}
