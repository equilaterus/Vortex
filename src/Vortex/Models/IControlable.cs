using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Models
{
    public interface IControlable
    {
        DateTime? CreationDate { get; set; }

        DateTime? ModificationDate { get; set; }
    }
}
