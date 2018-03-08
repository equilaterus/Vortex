using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine
{
    public interface IVortexExecutor<T> where T : class
    {
        Task<VortexData> ExecuteAsync(string vortexEvent, VortexData actionParams);

        void Initialize(VortexContext<T> vortexContext);
    }
}