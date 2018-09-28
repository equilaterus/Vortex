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
        public override void Initialize()
        {
            IsReturnAction = true;
            base.Initialize();            
        }

        public override async Task Execute()
        {
            var dataStorage = Context.DataStorage as IRelationalDataStorage<T>;

            List<T> result;

            var relationalParams = Params.GetMainEntityAs<RelationalQueryParams<T>>();
            if (relationalParams != null)
            {
                result = await dataStorage.FindAsync(
                    relationalParams
                );
            }
            else
            {
                // Try to get basic params
                var queryParams = Params.GetMainEntityAs<QueryParams<T>>();
                result = await dataStorage.FindAsync(
                   queryParams
               );
            }       

            Results = new VortexData(result);
        }

        public RelationalQueryForEntities(VortexContext<T> context) : base(context) { }
    }
}
