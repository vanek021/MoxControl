using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Models;

namespace MoxControl.Connect.Models.Entities
{
    public class ISOImage : BaseRecord
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ImageStorageMethod StorageMethod { get; set; }
        public string ImagePath { get; set; }
        public bool DownloadSuccess { get; set; }
        public ISOImageStatus Status { get; set; }
    }
}
