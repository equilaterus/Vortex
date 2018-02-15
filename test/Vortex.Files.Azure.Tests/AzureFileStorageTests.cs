﻿using Equilaterus.Vortex.Services.Tests;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Services.AzureStorage.Tests
{
    public class AzureFileStorageTests : FileStorageTests
    {
        static IOptions<AzureStorageConfig> GetDefaultOptions()
        {
            AzureStorageConfig config = new AzureStorageConfig() {
                ConnectionString = "UseDevelopmentStorage=true",
                ContainerName = "test",
                CreateIfNotExist = true
            };
            return Options.Create(config);
        }
        
        protected override async Task<IFileStorage> GetService()
        {
            using (Process process = new Process())                
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe";

                StartAndWaitForExit(process, "stop");
                StartAndWaitForExit(process, "clear all");
                StartAndWaitForExit(process, "start");
                                
                return new AzureFileStorage(GetDefaultOptions());
            }
        }

        private void StartAndWaitForExit(Process process, string arguments)
        {
            process.StartInfo.Arguments = arguments;
            process.Start();
            process.WaitForExit();
        }
    }
}