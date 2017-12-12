using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Models
{
    public interface IDateable
    {
        DateTime? Date { get; set; }
    }
}
