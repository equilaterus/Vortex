using Equilaterus.Vortex.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Actions
{
    public abstract class VortexReturnAction<TEntity, TReturn> where TEntity : class
    {
        public IVortexContext<TEntity> Context { get; private set; }

        protected readonly IGenericFilterFactory _FilterFactory;

        public VortexReturnAction(IVortexContext<TEntity> context, IGenericFilterFactory filterFactory)
        {
            Context = context;
            _FilterFactory = filterFactory;
        }

        public virtual void ApplyFilters(QueryParams<TEntity> queryParams)
        {
            if (queryParams.SkipFilters)            
                return;            

            var filters = _FilterFactory.GetFilters<TEntity>();
            foreach (var filter in filters)
            {
                filter.UpdateParams(queryParams);
            }
        }

        public abstract Task<TReturn> Execute(QueryParams<TEntity> queryParams);
    }
}
