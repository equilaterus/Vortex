using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Managers
{
    public interface IPersistanceManager<T>
    {
        Task ExecuteCommand(
            string vortexEvent, 
            VortexData entity);

        Task<List<T>> ExecuteQueryForEntities(
            string vortexEvent,
            VortexData queryParams);

        Task<int> ExecuteQueryForInt(
            string vortexEvent,
            VortexData queryParams);
    }
}
