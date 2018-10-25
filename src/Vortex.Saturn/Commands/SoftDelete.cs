using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Saturn.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Commands
{
    public class SoftDelete<T> : VortexAction<T> where T : class, ISoftDeleteable
    {
        public override async Task Execute(T entity, params object[] parameters)
        {
            entity.IsDeleted = true;
            await this.GetContext()
                .DataStorage.UpdateAsync(entity);
        }

        public SoftDelete(VortexContext<T> context) : base(context) { }
    }
}
