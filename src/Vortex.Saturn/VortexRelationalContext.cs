using Equilaterus.Vortex.Saturn.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Saturn
{
    public class VortexRelationalContext<T> : VortexContext<T> where T : class
    {
        public IRelationalDataStorage<T> RelationalDataStorage { get; protected set; }

        public VortexRelationalContext(
            IRelationalDataStorage<T> dataStorage, 
            IFileStorage fileStorage) : base(dataStorage, fileStorage)
        {
            RelationalDataStorage = dataStorage;
        }
    }
}
