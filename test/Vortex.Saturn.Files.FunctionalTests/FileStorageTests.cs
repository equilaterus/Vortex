using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.Tests
{
    public abstract class FileStorageTests
    {
        protected abstract Task<IFileStorage> GetService();

        [Fact]
        public async Task AFSTests_StoreFile()
        {
            IFileStorage service = await GetService();

            string fileName = "";            

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("whatever")))
            {
                fileName = await service.StoreFileAsync(memoryStream, ".txt");
            }

            Assert.True(fileName != null && fileName.Length > 0);
        }
        /*
        [Fact]
        public async Task AFSTests_FileNameFromPathNull()
        {
            IFileStorage service = await GetService();

            string path = null;
            Assert.Throws<ArgumentNullException>(() => AzureFileStorage.GetFileNameFromPath(path));
        }*/
    }
}
