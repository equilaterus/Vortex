using Equilaterus.Vortex.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services.MongoDB
{
    public class MongoDbDataStorage<T> : IDataStorage<T> where T : class
    {
        protected readonly IMongoDbContext _context;

        public MongoDbDataStorage(IMongoDbContext context)
        {
            _context = context;
        }


        public async Task<List<T>> FindAllAsync(params string[] includeProperties)
        {
            var result = await _context.GetCollection<T>().FindAsync(new BsonDocument());
            return await result.ToListAsync();
        }

        public async Task<List<T>> FindAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            int skip = 0, 
            int take = 0, 
            params string[] includeProperties)
        {
            IQueryable<T> result =  _context.GetCollection<T>().AsQueryable().Where(filter);
            result = orderBy(result);
            return result.ToList();
        }        

        public Task InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task InsertRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
        
        public Task<T> IncrementField(Expression<Func<T, bool>> filter, Expression<Func<T, int>> field, int quantity = 1)
        {
            throw new NotImplementedException();
        }
    }
}
