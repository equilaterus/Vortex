using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public interface IVortexEngine<T> where T : class
    {
        Task<VortexData> RaiseEventAsync(string vortexEvent, VortexData actionParams);
    }
}