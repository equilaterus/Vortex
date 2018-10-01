﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Commands
{
    public class InsertEntity<T> : GenericAction<T> where T : class
    {
        public override async Task Execute()
        {
            await Context.DataStorage.InsertAsync(
                Params.GetMainEntityAs<T>());
        }

        public InsertEntity(VortexContext<T> context) : base(context) { }
    }
}