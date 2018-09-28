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
    public class QueryForEntities<T> : GenericAction<T> where T : class
    {
        public override void Initialize()
        {
            IsReturnAction = true;
            base.Initialize();            
        }

        public override async Task Execute()
        {
            var dataStorage = Context.DataStorage;

            var result = await dataStorage.FindAsync(
                Params.GetMainEntityAs<QueryParams<T>>());

            Results = new VortexData(result);
        }

        public QueryForEntities(VortexContext<T> context) : base(context) { }
    }
}
