using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Managers;
using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public class AttacheableFileBehavior<T> : IAttacheableFileBehavior<T>
        where T : class, IAttacheableFile
    {
        protected readonly IPersistanceManager<T> _persistanceManager;

        public AttacheableFileBehavior(IPersistanceManager<T> persistanceManager)
        {
            _persistanceManager = persistanceManager;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            await _persistanceManager.DeleteEntityAsync(entity);
        }

        public virtual async Task InsertAsync(T entity, Stream stream, string extension)
        {
            await _persistanceManager.InsertEntityAsync(entity, stream, extension);
        }

        public virtual async Task UpdateAsync(T entity, Stream stream, string extension)
        {
            await _persistanceManager.UpdateEntityAsync(entity, stream, extension);
        }
    }
}
