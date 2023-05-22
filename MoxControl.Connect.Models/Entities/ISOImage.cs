using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public ISOImageStatus Status { get; set; }

        [Column(TypeName = "jsonb")]
        public ImageServerData? AvailableServerData { get; set; }
    }

    public class ImageServerData
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ImageAvailableServerDataItem> Items { get; set; } = new();
    }

    public class ImageAvailableServerDataItem
    {
        public long ServerId { get; set; }
        public VirtualizationSystem VirtualizationSystem { get; set; }
    }
}
