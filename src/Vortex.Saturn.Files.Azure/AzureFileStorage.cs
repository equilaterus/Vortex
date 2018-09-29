using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Services.AzureStorage
{
    public class AzureFileStorage : IFileStorage
    {
        private AzureStorageConfig _configuration;
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _container;

        public AzureFileStorage(IOptions<AzureStorageConfig> configuration)
        {
            _configuration = configuration.Value;

            _storageAccount = CloudStorageAccount.Parse(
                _configuration.ConnectionString);

            _blobClient = _storageAccount.CreateCloudBlobClient();
            _container = _blobClient.GetContainerReference(_configuration.ContainerName);

            if(_configuration.CreateIfNotExist)
            {
                _container.CreateIfNotExistsAsync().Wait();
            }
        }

        public async Task<bool> DeleteFileAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(GetFileNameFromPath(path));
            return await blockBlob.DeleteIfExistsAsync();
        }
                
        public string GenerateFileName()
        {
            return Guid.NewGuid().ToString("N");
        }

        public async Task<string> StoreFileAsync(Stream stream, string extension)
        {
            if (stream == null)
            {
                throw new ArgumentNullException();
            }

            string fileName = GenerateFileName() + extension;
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(fileName);

            await blockBlob.UploadFromStreamAsync(stream);            

            return blockBlob.Uri.ToString();               
        }

        public static string GetFileNameFromPath(string path)
        {
            if(path == null)
            {
                throw new ArgumentNullException();
            }
            var splitedPath = path.Split('/');
            return splitedPath[splitedPath.Length - 1];
        }       
    }
}
