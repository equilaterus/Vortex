using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Services.EFCore.Tests
{
    public class TestModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int Counter { get; set; }
    }
}
