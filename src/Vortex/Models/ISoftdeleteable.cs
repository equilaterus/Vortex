using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Models
{
    public interface ISoftDeleteable
    {
        bool IsDeleted { get; set; }
    }
}
