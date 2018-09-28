using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Queries;
using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Managers
{
    public static class AdjuntablePersistanceManager
    {
        public static async Task InsertEntityAsync<T>(
            this IPersistanceManager<T> p,
            T entity, 
            Stream stream,
            string extension)
            where T : class, IAttacheableFile
        {
            await p.ExecuteCommandAsync(
                VortexEvents.InsertEntity, 
                new VortexDataAdjuntable(
                    entity, 
                    stream, 
                    extension
                )
            );
        }

        public static async Task UpdateEntityAsync<T>(
            this IPersistanceManager<T> p,
            T entity,
            Stream stream,
            string extension)
            where T : class, IAttacheableFile
        {
            await p.ExecuteCommandAsync(
                VortexEvents.UpdateEntity,
                new VortexDataAdjuntable(
                    entity,
                    stream,
                    extension
                )
            );
        }
    }
}
