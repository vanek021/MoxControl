using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

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
