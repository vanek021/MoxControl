using MoxControl.Connect.Models.Enums;

namespace MoxControl.ViewModels.ImageViewModels
{
#pragma warning disable CS8618
    public class ImageDetailsViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ImageStorageMethod StorageMethod { get; set; }
        public string ImagePath { get; set; }
        public ISOImageStatus Status { get; set; }
        public List<ImageServerViewModel> Servers { get; set; } = new();
    }
#pragma warning restore CS8618
}
