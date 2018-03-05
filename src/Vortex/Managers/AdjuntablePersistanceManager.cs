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
        public static async Task<List<T>> InsertEntity<T>(
            this PersistanceManager<T> p,
            Stream stream)
            where T : class, IAdjuntable
        {
            return await p.ExecuteCommand(
                VortexEvents.InsertEntity, 
                new VortexData(
                    new RelationalQueryParams<T>()
                    {
                        Filter = filter,
                        OrderBy = orderBy,
                        Skip = skip,
                        Take = take,
                        IncludeProperties = includeProperties
                    }
                )
            );
        }
    }
}
