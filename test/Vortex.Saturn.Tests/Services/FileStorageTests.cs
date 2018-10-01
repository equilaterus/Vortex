using Equilaterus.Vortex.Saturn.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests.Services
{
    public abstract class FileStorageTests
    {
        protected abstract Task<IFileStorage> GetService();

        protected abstract Task<Stream> GetFileStream(IFileStorage service, string path);

        [Fact]
        public async Task AFSTests_StoreFile()
        {
            IFileStorage service = await GetService();

            string fileUrl = "";

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("whatever")))
            using (var memoryStream2 = new MemoryStream(Encoding.UTF8.GetBytes("whatever")))
            {
                fileUrl = await service.StoreFileAsync(memoryStream, ".txt");

                Assert.True(fileUrl != null && fileUrl.Length > 0);

                using (var fileStream = await GetFileStream(service, fileUrl))
                using (var md5CompA = MD5.Create())
                using (var md5CompB = MD5.Create())
                {
                    byte[] fileBuffer = new byte[fileStream.Length];
                    fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
                    var md5FileA = BitConverter.ToString(md5CompA.ComputeHash(memoryStream.ToArray()));
                    var md5FileB = BitConverter.ToString(md5CompB.ComputeHash(fileBuffer));

                    Assert.True(md5FileA == md5FileB);
                }
            }            
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
