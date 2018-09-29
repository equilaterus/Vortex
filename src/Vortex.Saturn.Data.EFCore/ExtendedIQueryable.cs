using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Equilaterus.Vortex.Saturn.Services.EFCore
{
    public static class ExtendedIQueryable
    {
        public static IQueryable<TEntity> AddIncludes<TEntity>(
            this IQueryable<TEntity> query, 
            params string[] includeProperties)                
                where TEntity : class
        {
            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }
            }
            
            return query;
        }
    }
}
