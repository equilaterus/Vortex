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

        public IGenericFilterFactory FilterFactory { get; private set; }

        public VortexReturnAction(IVortexContext<TEntity> context, IGenericFilterFactory filterFactory)
        {
            Context = context;
            FilterFactory = filterFactory;
        }        

        public virtual void ApplyFilters(QueryParams<TEntity> queryParams)
        {
            var filters = FilterFactory.GetFilters<TEntity>();
            foreach (var filter in filters)
            {
                filter.UpdateParams(queryParams);
            }
        }

        public abstract Task<TReturn> Execute(QueryParams<TEntity> queryParams, params object[] parameters);
    }
}
