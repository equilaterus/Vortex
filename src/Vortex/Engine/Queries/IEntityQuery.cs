using Equilaterus.Vortex.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine.Queries
{
    interface IEntityQuery<T> where T : class
    {
        void SetParams(QueryParams<T> filter);

        QueryParams<T> GetParams();

        Task<List<T>> Execute();
    }
}
