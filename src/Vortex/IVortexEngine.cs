using Equilaterus.Vortex.Filters;
using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public interface IVortexEngine<TEntity> where TEntity : class
    {
        Task<TReturn> RaiseEventAsync<TReturn>(
            string eventName,
            TEntity entity = null,
            QueryParams<TEntity> queryParams = null,
            params object[] parameters);
    }
}