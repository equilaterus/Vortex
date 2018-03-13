using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine.Commands
{
    public class UpdateAdjuntable<T> : GenericAction<T> where T : class, IAdjuntable
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

            await Context.FileStorage.DeleteFileAsync(
                Params.GetMainEntityAs<T>().FileUrl);

            string fileUrl = await Context.FileStorage.StoreFileAsync(
                                adjuntableParams.File,
                                adjuntableParams.Extension);

            Params.GetMainEntityAs<T>().FileUrl = fileUrl;
        }

        public UpdateAdjuntable(VortexContext<T> context) : base(context) { }
    }
}
