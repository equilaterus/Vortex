using Equilaterus.Vortex.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine
{
    public class VortexContext<T> where T : class
    {
        public IDataStorage<T> DataStorage { get; protected set; }

        public IFileStorage FileStorage { get; protected set; }

        public VortexContext() { }

        public VortexContext(
            IDataStorage<T> dataStorage, 
            IFileStorage fileStorage) 
        {
            DataStorage = dataStorage;
            FileStorage = fileStorage;
        }
    }
}
