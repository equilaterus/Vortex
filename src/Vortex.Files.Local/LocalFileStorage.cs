using System;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services.LocalStorage
{
    public class LocalFileStorage : IFileStorage
    {
        private LocalStorageConfig Configuration;
        private string Path;

        public LocalFileStorage(IOptions<LocalStorageConfig> configuration)
        {            
            Path = configuration.Value.LocalPath;
        }

        public async Task<bool> DeleteFileAsync(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }

        public string GenerateFileName()
        {
            return Guid.NewGuid().ToString("N");
        }

        public async Task<string> StoreFileAsync(Stream stream, string extension)
        {
            if (stream == null || extension == null)
            {
                throw new ArgumentNullException();
            }

            string fileName = GenerateFileName() + extension;
            string filePath = Path + fileName;
            
            using (var fileStream = File.Create(filePath))
            {
                await stream.CopyToAsync(fileStream);                
            }
                
            return filePath;               
        }
    }
}
