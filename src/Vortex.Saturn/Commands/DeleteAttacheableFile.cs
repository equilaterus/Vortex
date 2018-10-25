using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Saturn.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Commands
{
    public class DeleteAttacheableFile<T> : VortexAction<T> where T : class, IAttacheableFile
    {
        public override async Task Execute(T entity, params object[] parameters)
        {
            await this.GetContext().FileStorage.DeleteFileAsync(entity.FileUrl);

            entity.FileUrl = null;
        }

        public DeleteAttacheableFile(VortexContext<T> context) : base(context) { }
    }
}
