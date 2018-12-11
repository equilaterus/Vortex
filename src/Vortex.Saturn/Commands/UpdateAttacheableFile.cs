using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Saturn.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Commands
{
    public class UpdateAttacheableFile<T> : VortexAction<T> where T : class, IAttacheableFile
    {
        public override async Task Execute(T entity, params object[] parameters)
        {
            if (parameters == null)
                return;

            var adjuntableParams = parameters[0] as AttachedFile;
            if (adjuntableParams == null)
                throw new Exception("Incorrect params");

            if (adjuntableParams.File == null)
                return;

            await this.GetContext()
                .FileStorage.DeleteFileAsync(entity.FileUrl);

            string fileUrl = await this.GetContext()
                                .FileStorage.StoreFileAsync(
                                    adjuntableParams.File,
                                    adjuntableParams.Extension);

            entity.FileUrl = fileUrl;
        }

        public UpdateAttacheableFile(VortexContext<T> context) : base(context) { }
    }
}
