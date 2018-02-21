using Equilaterus.Vortex.Services;
using Equilaterus.Vortex.Engine.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine.Queries
{
    public class RelationalQueryForEntities<T> : GenericAction<T> where T : class
    {
        public override async Task Execute()
        {
            var result = await Context.DataStorage.FindAsync(
                Params.GetMainEntityAs<RelationalQueryParams<T>>());

            Results = new VortexData(result);
        }

        public RelationalQueryForEntities(VortexContext<T> context) : base(context) { }
    }
}
