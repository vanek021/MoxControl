using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Entities
{
    public class ISOImage : BaseRecord
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ImageStorageMethod StorageMethod { get; set; }
        public string ImagePath { get; set; }
        public bool DownloadSuccess { get; set; }
    }
}
