using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vortex.Tests
{
    public class TestModel
    {
        public int Id { get; set; }
    }

    public class AdjuntableTestModel : IAdjuntable
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }

        public AdjuntableTestModel(string fileUrl)
        {
            FileUrl = fileUrl;
        }

        public AdjuntableTestModel() { }
    }
}
