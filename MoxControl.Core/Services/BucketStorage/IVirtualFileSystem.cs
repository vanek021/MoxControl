using Microsoft.Extensions.FileProviders;

namespace MoxControl.Core.Services.BucketStorage
{
    public interface IVirtualFileSystem
    {
        IEnumerable<IFileInfo> EnumerateChildObjects(string path);
        IFileInfo GetObject(string path);
    }
}
