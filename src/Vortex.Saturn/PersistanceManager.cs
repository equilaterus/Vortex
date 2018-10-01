using Equilaterus.Vortex.Saturn.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn
{
    public class PersistanceManager<T> : IPersistanceManager<T> where T : class
    {
        protected readonly IDataStorage<T> _dataStorage;
        protected readonly IVortexEngine<T> _vortexExecutor;

        public PersistanceManager(
            IDataStorage<T> dataStorage,
            IVortexEngine<T> vortexExecutor)
        {
            _dataStorage = dataStorage;
            _vortexExecutor = vortexExecutor;
        }

        public PersistanceManager(
            IDataStorage<T> dataStorage, 
            IFileStorage fileStorage,
            IVortexEngine<T> vortexExecutor)
        {
            _dataStorage = dataStorage;
            _vortexExecutor = vortexExecutor;
        }

        public async Task ExecuteCommandAsync(
            string vortexEvent, 
            VortexData entity)
        {
            if (string.IsNullOrEmpty(vortexEvent))
            {
                throw new ArgumentNullException(nameof(vortexEvent));
            }
            
            await _vortexExecutor.RaiseEventAsync(vortexEvent, entity);
        }

        public async Task<List<T>> ExecuteQueryForEntitiesAsync(
            string vortexEvent, 
            VortexData queryParams)
        {
            if (string.IsNullOrEmpty(vortexEvent))
            {
                throw new ArgumentNullException(nameof(vortexEvent));
            }

            var result = await _vortexExecutor.RaiseEventAsync(
                vortexEvent,
                queryParams);

            return result.GetMainEntityAs<List<T>>();
        }

        public async Task<int> ExecuteQueryForIntAsync(string vortexEvent, VortexData queryParams)
        {
            if (string.IsNullOrEmpty(vortexEvent))
            {
                throw new ArgumentNullException(nameof(vortexEvent));
            }

            var result = await _vortexExecutor.RaiseEventAsync(
                vortexEvent,
                queryParams);

            return (int)result.Entity;
        }
    }
}
