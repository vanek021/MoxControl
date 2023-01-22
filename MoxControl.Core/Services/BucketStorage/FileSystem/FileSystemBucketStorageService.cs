using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Core.Services.BucketStorage.FileSystem
{
    public class FileSystemBucketStorageService : IBucketStorageService
    {
        public string RootPath { get; }
        public string MainDirectory { get; } = "uploads";

        public FileSystemBucketStorageService(string ServerWebRootPath)
        {
            RootPath = ServerWebRootPath;
        }

        public IBucket GetBucket(string bucketName)
        {
            bucketName = bucketName.ToLower();
            bucketName = bucketName.Replace('\\', '/');
            return new FileSystemBucket(this, bucketName);
        }
    }
}
