using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Models
{
    public interface IRangeDateable
    {
        DateTime? BeginDate { get; set; }

        DateTime? EndDate { get; set; }
    }
}
