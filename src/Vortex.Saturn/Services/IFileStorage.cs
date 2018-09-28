using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Services
{
    public interface IFileStorage
    {
        Task<string> StoreFileAsync(Stream stream, string extension);

        Task<bool> DeleteFileAsync(string path);

        string GenerateFileName();
    }
}
