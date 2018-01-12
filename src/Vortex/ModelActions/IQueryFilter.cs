using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.ModelActions
{
    interface IQueryFilter<T>
    {
        Expression<Func<T, bool>> Do(Expression<Func<T, bool>> filter);
    }
}
