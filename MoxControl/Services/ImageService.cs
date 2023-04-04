using AutoMapper;
using MoxControl.Connect.Data;
using MoxControl.Connect.Services;
using MoxControl.ViewModels.ImageViewModels;

namespace MoxControl.Services
{
    public class ImageService
    {
        private readonly IMapper _mapper;
        private readonly ConnectDatabase _connectDatabase;

        public ImageService(IMapper mapper, ConnectDatabase connectDatabase)
        {
            _mapper = mapper;
            _connectDatabase = connectDatabase;
        }

        public async Task<List<ImageViewModel>> GetImageViewModelsAsync()
        {
            var images = await _connectDatabase.ISOImages.GetAll();

            var imageVms = _mapper.Map<List<ImageViewModel>>(images);

            return imageVms;
        }
    }
}
