﻿using AutoMapper;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Services;
using MoxControl.ViewModels.ImageViewModels;

namespace MoxControl.Services
{
    public class ImageService
    {
        private readonly IMapper _mapper;
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly ImageManager _imageManager;

        public ImageService(IMapper mapper, IConnectServiceFactory connectServiceFactory, ImageManager imageManager)
        {
            _mapper = mapper;
            _connectServiceFactory = connectServiceFactory;
            _imageManager = imageManager;
        }

        public async Task<ImageIndexViewModel> GetImageIndexViewModelAsync()
        {
            var images = await _imageManager.GetAllAsync();

            var imageIndexVm = new ImageIndexViewModel
            {
                Images = _mapper.Map<List<ImageViewModel>>(images)
            };

            return imageIndexVm;
        }

        public async Task<ImageDetailsViewModel?> GetImageDetailsViewModelAsync(long imageId)
        {
            var image = await _imageManager.GetByIdAsync(imageId);

            if (image is null)
                return null;

            var imageDetailsVm = _mapper.Map<ImageDetailsViewModel>(image);

            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();

                foreach (var server in servers)
                {
                    var serverVm = _mapper.Map<ImageServerViewModel>(server);

                    if (server.ImageData is not null && server.ImageData.ImageIds.Contains(image.Id))
                        serverVm.IsImageDelivered = true;

                    imageDetailsVm.Servers.Add(serverVm);
                }
            }

            return imageDetailsVm;
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
