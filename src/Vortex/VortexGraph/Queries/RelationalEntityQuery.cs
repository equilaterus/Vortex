using Equilaterus.Vortex.Services;
using Equilaterus.Vortex.VortexGraph.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Commands.Queries
{
    public class RelationalEntityQuery<T> : IEntityQuery<T> where T : class
    {
        private readonly IRelationalDataStorage<T> _dataStorage;

        private RelationalQueryParams<T> _queryParams;

        public RelationalEntityQuery(IRelationalDataStorage<T> dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public void SetParams(QueryParams<T> queryParams)
        {
            if (queryParams is RelationalQueryParams<T> relParams)
            {
                _queryParams = relParams;
            }
            else
            {
                _queryParams = new RelationalQueryParams<T>(queryParams);
            }           
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
