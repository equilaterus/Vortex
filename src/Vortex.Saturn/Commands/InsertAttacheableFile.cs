using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Saturn.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Commands
{
    public class InsertAttacheableFile<T> : VortexAction<T> where T : class, IAttacheableFile
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

            string fileUrl = await this.GetContext().FileStorage.StoreFileAsync(
                                adjuntableParams.File,
                                adjuntableParams.Extension);

            entity.FileUrl = fileUrl;
        }

        public InsertAttacheableFile(VortexContext<T> context) : base(context) { }
    }
}
