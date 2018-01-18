using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Services.AzureStorage
{
    public class AzureStorageConfig 
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string ContainerName { get; set; }
        public string ConnectionString { get; set; }
        public bool CreateIfNotExist { get; set; }
    }
}
