using AutoMapper;
using MoxControl.Connect.Data;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Services;
using MoxControl.ViewModels.ImageViewModels;
using MoxControl.ViewModels.ServerViewModels;

namespace MoxControl.Services
{
    public class ImageService
    {
        private readonly IMapper _mapper;
        private readonly ImageManager _imageManager;

        public ImageService(IMapper mapper, ImageManager imageManager)
        {
            _mapper = mapper;
            _imageManager = imageManager;
        }

        public async Task<ImageIndexViewModel> GetImageIndexViewModelAsync()
        {
            var images = await _imageManager.GetAllAsync();

            var imageIndexVm = new ImageIndexViewModel();

            imageIndexVm.Images = _mapper.Map<List<ImageViewModel>>(images);

            return imageIndexVm;
        }

        public async Task<ImageViewModel> GetImageViewModelAsync(long id)
        {
            var image = await _imageManager.GetByIdAsync(id);

            var imageVm = _mapper.Map<ImageViewModel>(image);

            return imageVm;
        }

        public async Task<bool> CreateAsync(ImageViewModel viewModel)
        {
            var image = _mapper.Map<ISOImage>(viewModel);

            var result = await _imageManager.CreateAsync(image);

            return result;
        }

        public async Task<bool> UpdateAsync(ImageViewModel viewModel)
        {
            var image = _mapper.Map<ISOImage>(viewModel);

            var result = await _imageManager.UpdateAsync(image);

            return result;
        }

        public async Task<bool> DeleteAsync(long imageId)
        {
            var result = await _imageManager.DeleteAsync(imageId);

            return result;
        }
    }
}
