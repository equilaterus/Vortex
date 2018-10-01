using Equilaterus.Vortex.Saturn.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Commands
{
    public class UpdateAttacheableFile<T> : GenericAction<T> where T : class, IAttacheableFile
    {
        public override async Task Execute()
        {
            var adjuntableParams = Params as VortexDataAttacheable;
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

        public UpdateAttacheableFile(VortexContext<T> context) : base(context) { }
    }
}
