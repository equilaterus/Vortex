using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.AzureStorage.Tests
{
    public class AzureFileStorageTests
    {
        static IOptions<AzureStorageConfig> GetDefaultOptions()
        {
            AzureStorageConfig config = new AzureStorageConfig() {
                ConnectionString = "UseDevelopmentStorage=true",
                ContainerName = "test" };
            return Options.Create(config);
        }

        [Fact]
        public async Task AFSTests_StoreFile()
        {
            string fileName = "";
            AzureFileStorage service = new AzureFileStorage(GetDefaultOptions());

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("whatever")))
            {
                fileName = await service.StoreFileAsync(memoryStream, ".txt");
            }

            Assert.True(fileName != null && fileName.Length > 0);

            var result = await service.DeleteFileAsync(fileName);
            Assert.True(result);
        }
        
        [Fact]
        public void AFSTests_FileNameFromPathNull()
        {
            string path = null;
            Assert.Throws<ArgumentNullException>(() => AzureFileStorage.GetFileNameFromPath(path));
        }
    }
}
