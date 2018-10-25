﻿using Equilaterus.Vortex.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Commands
{
    public class DeleteEntity<T> : VortexAction<T> where T : class
    {
        public override async Task Execute(T entity, params object[] parameters)
        {
            await this.GetContext()
                .DataStorage.DeleteAsync(entity);
        }

        public DeleteEntity(VortexContext<T> context) : base(context) { }
    }
}
