using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services
{
    public interface INonRelationalDataStorage<T> : IDataStorage<T> where T : class
    {       
        Task<T> IncrementField(
            Expression<Func<T, bool>> filter, 
            Expression<Func<T, int>> field, 
            int quantity = 1);
    }
}
