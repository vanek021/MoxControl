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

            CreateMap<ISOImage, ImageDetailsViewModel>();
            CreateMap<ImageDetailsViewModel, ISOImage>();

            CreateMap<BaseServer, ImageServerViewModel>();
            CreateMap<ImageServerViewModel, BaseServer>();
        }
    }
}
