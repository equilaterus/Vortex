using Equilaterus.Vortex.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public class RelationalCrudBehavior<T>
        : CrudBehavior<T>,
        IRelationalCrudBehavior<T>        
        where T : class
    {
        public async Task<List<T>> FindAllAsync(params string[] includeProperties)
        {
            return await PersistanceManager.FindAllAsync(includeProperties);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            int skip = 0, int take = 0, params string[] includeProperties)
        {
            return await PersistanceManager.FindAsync(filter, orderBy, skip, take, includeProperties);
        }
    }
}
