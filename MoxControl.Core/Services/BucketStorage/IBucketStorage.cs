using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Core.Services.BucketStorage
{
    public interface IBucketStorageService
    {
        IBucket GetBucket(string bucketName);
    }

    public interface IBucketStorageFileProvider
    {
        PathString BasePath { get; }
        IFileProvider FileProvider { get; }
    }

    public interface IBucket
    {
        bool ContainsObject(string fileName);
        void ReadObject(string fileName, Stream readStream);
        void WriteObject(string fileName, Stream writeStream);
        void DeleteObject(string fileName);
        string GetPublicURL(string fileName);
    }
}
