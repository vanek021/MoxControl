using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Core.Services.BucketStorage
{
    public interface IVirtualFileSystem
    {
        IEnumerable<IFileInfo> EnumerateChildObjects(string path);
        IFileInfo GetObject(string path);
    }
}
