using Equilaterus.Vortex.Services;
using Equilaterus.Vortex.Engine.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine.Commands
{
    public class UpdateEntity<T> : GenericAction<T> where T : class
    {
        public override async Task Execute()
        {
            await Context.DataStorage.UpdateAsync(
                Params.GetMainEntityAs<T>());
        }

        public UpdateEntity(VortexContext<T> context) : base(context) { }
    }
}
