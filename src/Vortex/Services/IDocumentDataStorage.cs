using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services
{
    /// <summary>
    /// Represents the data storage for document oriented databases.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDocumentDataStorage<T> : IDataStorage<T> where T : class
    {       
        Task<T> IncrementField(
            Expression<Func<T, bool>> filter, 
            Expression<Func<T, int>> field, 
            int quantity = 1);
    }
}
