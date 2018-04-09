using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public interface IAttacheableFileBehavior<T>
        where T : IAttacheableFile
    {
        Task InsertAsync(T entity, Stream stream, string extension);

        Task UpdateAsync(T entity, Stream stream, string extension);

        Task DeleteAsync(T entity);
    }
}
