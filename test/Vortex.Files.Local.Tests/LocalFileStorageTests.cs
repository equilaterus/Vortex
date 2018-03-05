using Equilaterus.Vortex.Services.Tests;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services.LocalStorage.Tests
{
    public class LocalFileStorageTests : FileStorageTests
    {
        static IOptions<LocalStorageConfig> GetDefaultOptions()
        {
            
            LocalStorageConfig config = new LocalStorageConfig()
            {
                LocalPath =  Environment.CurrentDirectory + @"\LocalFiles"
            };
                        
            return Options.Create(config);
        }

        protected override async Task<IFileStorage> GetService()
        {
            Directory.CreateDirectory(Environment.CurrentDirectory + @"\LocalFiles");
            var files = Directory.GetFiles(Environment.CurrentDirectory + @"\LocalFiles");
            foreach (var file in files)
            {
                File.Delete(file);
            }

            return new LocalFileStorage(GetDefaultOptions());
        }
    }
}
