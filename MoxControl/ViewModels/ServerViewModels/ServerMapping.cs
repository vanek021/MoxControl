using AutoMapper;
using MoxControl.Connect.Models.Entities;

namespace MoxControl.ViewModels.ServerViewModels
{
    public class ServerMapping : Profile
    {
        public ServerMapping()
        {
            CreateMap<ServerViewModel, BaseServer>();
            CreateMap<BaseServer, ServerViewModel>();

            CreateMap<BaseServer, ServerDetailsViewModel>()
                .ForMember(dest => dest.Machines, opt => opt.Ignore())
                .ForMember(dest => dest.WebUISource, opt => opt.Ignore());

            CreateMap<ServerShortViewModel, BaseServer>();
            CreateMap<BaseServer, ServerShortViewModel>();
        }
    }
}
