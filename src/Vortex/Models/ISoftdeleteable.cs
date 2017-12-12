using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Models
{
    public interface ISoftdeleteable
    {
        bool IsDeleted { get; set; }
    }
}
