using Equilaterus.Vortex.Helpers;
using Equilaterus.Vortex.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.ModelActions
{
    public class ActivableFilter<T> : IQueryFilter<T> where T : IActivable
    {
        public Expression<Func<T, bool>> Do(Expression<Func<T, bool>> filter)
        {
            var inner = PredicateBuilder.New<T>();
            inner = inner.Start(e => e.IsActive);
            return inner.And(filter);
        }
    }
}
