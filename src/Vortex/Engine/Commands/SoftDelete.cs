using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine.Commands
{
    public class SoftDelete<T> : GenericAction<T> where T : class, ISoftDeleteable
    {
        public override void Initialize()
        {
            IsDefaultAction = false;
            PreventDefault = true;
            Priority = 1;
        }

        public override async Task Execute()
        {
            Params.GetMainEntityAs<T>().IsDeleted = true;
            await Context.DataStorage.UpdateAsync(Params.GetMainEntityAs<T>());
        }

        public SoftDelete(VortexContext<T> context) : base(context)
        {            
        }
    }
}
