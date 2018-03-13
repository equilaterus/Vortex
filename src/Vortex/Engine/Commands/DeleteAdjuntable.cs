using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine.Commands
{
    public class DeleteAdjuntable<T> : GenericAction<T> where T : class, IAdjuntable
    {
        public override void Initialize()
        {
            PreventDefault = false;
            base.Initialize();
        }

        public override async Task Execute()
        {
            await Context.FileStorage.DeleteFileAsync(
                Params.GetMainEntityAs<T>().FileUrl);

            Params.GetMainEntityAs<T>().FileUrl = null;
        }

        public DeleteAdjuntable(VortexContext<T> context) : base(context) { }
    }
}
