using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LinqKit;
using System.Text;

namespace Equilaterus.Vortex.Helpers
{
    static class ExtendedExpression
    {
        public static Expression<Func<T,bool>> Bind<T>(
            this Expression<Func<T, bool>> expressionA,
            Expression<Func<T, bool>> expressionB) 
            where T : class
        {
            if (expressionA == null)
            {
                return expressionB;
            }
            return expressionB == null ? expressionA : expressionB.And(expressionA);
        }
    }
}
