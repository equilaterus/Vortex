using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Queries;
using Equilaterus.Vortex.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Equilaterus.Vortex.Engine.Configuration.CommandBindings;

namespace Equilaterus.Vortex.Managers
{
    public class PersistanceManager<T> : IPersistanceManager<T> where T : class
    {
        protected readonly IDataStorage<T> _dataStorage;
        protected readonly VortexExecutor<T> _vortexExecutor;


        public PersistanceManager(
            IDataStorage<T> dataStorage, 
            IFileStorage fileStorage, 
            VortexExecutor<T> vortexExecutor)
        {
            _dataStorage = dataStorage;
            _vortexExecutor = vortexExecutor;
            _vortexExecutor.Initialize(new VortexContext<T>(dataStorage, fileStorage));
        }

        public async Task ExecuteCommand(string vortexEvent, T entity)
        {
             await _vortexExecutor.Execute(vortexEvent, new VortexData(entity));
        }

        public async Task<List<T>> ExecuteQuery(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int skip = 0,
            int take = 0)
        {
            var result = await _vortexExecutor.Execute(
                VortexEvents.RelationalQueryForEntities.ToString(),
                new VortexData(new QueryParams<T>()));

            return result.GetMainEntityAs<List<T>>();
        }
    }
}
