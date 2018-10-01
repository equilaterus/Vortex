using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn
{
    public interface IPersistanceManager<T>
    {
        Task ExecuteCommandAsync(
            string vortexEvent, 
            VortexData entity);

        Task<List<T>> ExecuteQueryForEntitiesAsync(
            string vortexEvent,
            VortexData queryParams);

        Task<int> ExecuteQueryForIntAsync(
            string vortexEvent,
            VortexData queryParams);
    }
}
