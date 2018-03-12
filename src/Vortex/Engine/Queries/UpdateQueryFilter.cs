using Equilaterus.Vortex.Engine.Queries.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine.Queries
{
    public class UpdateQueryFilter<T> : GenericAction<T> where T : class
    {
        public override void Initialize()
        {
            Priority = 1;
        }

        public override async Task Execute()
        {
            if (Params.GetMainEntityAs<QueryParams<T>>().SkipFilters)
            {
                return;
            }

            var filters = GenericFilterFactory.GetInstance().GetFilters<T>();
            foreach (var filter in filters)
            {
                filter.UpdateParams(Params.GetMainEntityAs<QueryParams<T>>());
            }            
        }

        public UpdateQueryFilter(VortexContext<T> context)
        {
            Context = context;
        }
    }
}
