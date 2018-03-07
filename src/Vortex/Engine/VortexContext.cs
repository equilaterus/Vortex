using Equilaterus.Vortex.Engine.Queries.Filters;
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

        public GenericFilterFactory FilterFactory { get; protected set; }

        public VortexContext() { }

        public VortexContext(
            IDataStorage<T> dataStorage, 
            IFileStorage fileStorage,
            GenericFilterFactory filterFactory) 
        {
            DataStorage = dataStorage;
            FileStorage = fileStorage;
            FilterFactory = filterFactory;
        }
    }
}
