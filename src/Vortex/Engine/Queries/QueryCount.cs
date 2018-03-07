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
    public class QueryCount<T> : GenericAction<T> where T : class
    {
        public override void Initialize()
        {
            IsReturnAction = true;
            base.Initialize();            
        }

        public override async Task Execute()
        {
            var dataStorage = Context.DataStorage;

            var result = await dataStorage.Count(
                Params.GetMainEntityAs<QueryParams<T>>().Filter);

            Results = new VortexData(result);
        }

        public QueryCount(VortexContext<T> context) : base(context) { }
    }
}
