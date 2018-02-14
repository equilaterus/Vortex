using Equilaterus.Vortex.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine.Queries
{
    public class EntityQuery<T> : IEntityQuery<T> where T : class
    {
        private readonly IDataStorage<T> _dataStorage;

        private QueryParams<T> _queryParams;

        public EntityQuery(IDataStorage<T> dataStorage)
        {
            _dataStorage = dataStorage;
        }        

        public void SetParams(QueryParams<T> queryParams)
        {
            _queryParams = queryParams;
        }

        public QueryParams<T> GetParams()
        {
            return _queryParams;
        }

        public async Task<List<T>> Execute()
        {
            return await _dataStorage.FindAsync(_queryParams);
        }
    }
}
