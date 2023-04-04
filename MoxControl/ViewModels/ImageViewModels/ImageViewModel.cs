using MoxControl.Connect.Models.Enums;

namespace MoxControl.ViewModels.ImageViewModels
{
    public class ImageViewModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ImageStorageMethod StorageMethod { get; set; }
        public string ImagePath { get; set; }
    }
}
