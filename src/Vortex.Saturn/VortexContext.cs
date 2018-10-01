using Equilaterus.Vortex.Saturn.Services;

namespace Equilaterus.Vortex.Saturn
{
    public class VortexContext<T> : IVortexContext<T> where T : class
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
