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
    public class MongoDbDataStorage<T> : IDocumentDataStorage<T> where T : MongoDbEntity
    {
        protected readonly IMongoDbContext _context;

        public MongoDbDataStorage(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> FindAllAsync()
        {
            var result = await _context.GetCollection<T>().FindAsync(new BsonDocument());
            return await result.ToListAsync();
        }

        public async Task<List<T>> FindAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            int skip = 0, 
            int take = 0)
        {
            if (skip < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip));
            }

            if (take < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(take));
            }

            IQueryable<T> result =  _context.GetCollection<T>().AsQueryable();

            if (filter != null)
            {
                result = result.Where(filter);
            }

            if (orderBy != null)
            {
                result = orderBy(result);
            }

            if (skip > 0)
            {
                result = result.Skip(skip);
            }

            if (take > 0)
            {
                result = result.Take(take);
            }
            
            return await ((IMongoQueryable<T>)result).ToListAsync();
        }        

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.GetCollection<T>().InsertOneAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _context.GetCollection<T>().InsertManyAsync(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.GetCollection<T>().ReplaceOneAsync(c => c.Id == entity.Id, entity);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            foreach (var entity in entities)
            {
                await _context.GetCollection<T>().ReplaceOneAsync(c => c.Id == entity.Id, entity);
            }
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.GetCollection<T>().DeleteOneAsync(c => c.Id == entity.Id);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var filter = Builders<T>.Filter.In(e => e.Id, entities.Select(l => l.Id));
            await _context.GetCollection<T>().DeleteManyAsync(filter);
        }
        
        public async Task<T> IncrementFieldAsync(
            Expression<Func<T, bool>> filter, 
            Expression<Func<T, int>> field, 
            int quantity = 1)
        {
            var update = Builders<T>.Update.Inc(field, quantity);
            var result = await _context.GetCollection<T>()
                .FindOneAndUpdateAsync(
                    filter, 
                    update, 
                    new FindOneAndUpdateOptions<T>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
            return result;
        }

        public async Task IncrementFieldRangeAsync(
            Expression<Func<T, bool>> filter, 
            Expression<Func<T, int>> field, 
            int quantity = 1)
        {
            var update = Builders<T>.Update.Inc(field, quantity);
            await _context.GetCollection<T>().UpdateManyAsync(filter, update);
        }
    }
}
