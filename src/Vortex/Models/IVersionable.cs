using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Models
{
    public interface IVersionable<T> : IBaseModel<T>
    {
        T OriginalId { get; set; }

        int Version { get; set; }
    }
}
