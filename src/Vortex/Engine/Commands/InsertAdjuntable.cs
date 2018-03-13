using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine.Commands
{
    public class InsertAdjuntable<T> : GenericAction<T> where T : class, IAdjuntable
    {
        public override async Task Execute()
        {
            VortexDataAdjuntable adjuntableParams = Params as VortexDataAdjuntable;
            if (adjuntableParams == null)
            {
                throw new Exception("Incorrect params");
            }

            if (adjuntableParams.File == null)
            {
                return;
            }

            string fileUrl = await Context.FileStorage.StoreFileAsync(
                                adjuntableParams.File,
                                adjuntableParams.Extension);

            Params.GetMainEntityAs<T>().FileUrl = fileUrl;
        }

        public InsertAdjuntable(VortexContext<T> context) : base(context) { }
    }
}
