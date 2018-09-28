using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Saturn.Models
{
    public interface IBaseModel<T>
    {
        T Id { get; set; }
    }
}
