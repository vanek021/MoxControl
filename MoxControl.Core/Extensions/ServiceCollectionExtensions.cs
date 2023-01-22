using Microsoft.Extensions.DependencyInjection;
using MoxControl.Core.Services.BucketStorage.FileSystem;
using MoxControl.Core.Services.BucketStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileSystemBucketStorage(this IServiceCollection serviceCollection, string ServerWebRootPath, string defaultBucketName)
        {
            var storageService = new FileSystemBucketStorageService(ServerWebRootPath);
            serviceCollection.AddSingleton<IBucketStorageService>(storageService);

            var bucket = storageService.GetBucket(defaultBucketName);
            serviceCollection.AddSingleton<IBucket>(bucket);

            return serviceCollection;
        }
    }
}
