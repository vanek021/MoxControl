using MoxControl.Connect.Services;

namespace MoxControl.Services
{
    public class ImageService
    {
        private readonly ImageManager _imageManager;

        public ImageService(ImageManager imageManager)
        {
            _imageManager = imageManager;
        }
    }
}
