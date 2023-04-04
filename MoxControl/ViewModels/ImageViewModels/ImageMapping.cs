using AutoMapper;
using MoxControl.Connect.Models.Entities;

namespace MoxControl.ViewModels.ImageViewModels
{
    public class ImageMapping : Profile
    {
        public ImageMapping()
        {
            CreateMap<ImageViewModel, ISOImage>();
            CreateMap<ISOImage, ImageViewModel>();
        }
    }
}
