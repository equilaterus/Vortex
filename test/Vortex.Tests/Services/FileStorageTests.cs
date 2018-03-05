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

            string fileUrl = "";            

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("whatever")))
            {
                fileUrl = await service.StoreFileAsync(memoryStream, ".txt");                
            }

            Assert.True(fileUrl != null && fileUrl.Length > 0);
        }
        
        [Fact]
        public async Task AFSTests_StoreFileFromNullStream()
        {
            IFileStorage service = await GetService();
            
            await Assert.ThrowsAsync<ArgumentNullException>( () => service.StoreFileAsync(null, null));
        }

        [Fact]
        public async Task AFSTests_CreateAndDeleteFile()
        {
            IFileStorage service = await GetService();

            string fileUrl = "";

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("whatever")))
            {
                fileUrl = await service.StoreFileAsync(memoryStream, ".txt");
            }

            Assert.True(fileUrl != null && fileUrl.Length > 0);

            bool deleteResult = await service.DeleteFileAsync(fileUrl);

            Assert.True(deleteResult);
        }

        [Fact]
        public async Task AFSTests_DeleteNonExistentFile()
        {
            IFileStorage service = await GetService();
                        
            bool deleteResult = await service.DeleteFileAsync("");

            Assert.True(!deleteResult);
        }
    }
}
