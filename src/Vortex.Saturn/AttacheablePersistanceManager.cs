using Equilaterus.Vortex.Saturn.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn
{
    public static class AttacheablePersistanceManager
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
                new VortexDataAttacheable(
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
                new VortexDataAttacheable(
                    entity,
                    stream,
                    extension
                )
            );
        }
    }
}
