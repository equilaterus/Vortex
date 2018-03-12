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
    public class AdjuntableBehavior<T> : IAdjuntableBehavior<T>
        where T : class, IAdjuntable
    {
        protected IPersistanceManager<T> PersistanceManager { get; private set; }

        public virtual async Task DeleteAsync(T entity)
        {
            await PersistanceManager.DeleteEntityAsync(entity);
        }

        public virtual async Task InsertAsync(T entity, Stream stream, string extension)
        {
            await PersistanceManager.InsertEntityAsync(entity, stream, extension);
        }

        public virtual async Task UpdateAsync(T entity, Stream stream, string extension)
        {
            await PersistanceManager.UpdateEntityAsync(entity, stream, extension);
        }
    }
}
