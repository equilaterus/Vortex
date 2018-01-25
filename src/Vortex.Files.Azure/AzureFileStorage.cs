using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services.AzureStorage
{
    public class AzureFileStorage : IFileStorage
    {
        private AzureStorageConfig Configuration;
        private CloudStorageAccount StorageAccount;
        private CloudBlobClient BlobClient;
        private CloudBlobContainer Container;

        public AzureFileStorage(IOptions<AzureStorageConfig> configuration)
        {            
            Configuration = configuration.Value;

            StorageAccount = CloudStorageAccount.Parse(
                Configuration.ConnectionString);

            BlobClient = StorageAccount.CreateCloudBlobClient();
            Container = BlobClient.GetContainerReference(Configuration.ContainerName);

            if(Configuration.CreateIfNotExist)
            {
                Container.CreateIfNotExistsAsync().Wait();
            }
        }

        public async Task<bool> DeleteFileAsync(string path)
        {
            CloudBlockBlob blockBlob = Container.GetBlockBlobReference(GetFileNameFromPath(path));
            return await blockBlob.DeleteIfExistsAsync();
        }

        public string GenerateFileName()
        {
            return Guid.NewGuid().ToString("N");
        }

        public async Task<string> StoreFileAsync(Stream stream, string extension)
        {
            string fileName = GenerateFileName() + extension; ;
            CloudBlockBlob blockBlob = Container.GetBlockBlobReference(fileName);

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
