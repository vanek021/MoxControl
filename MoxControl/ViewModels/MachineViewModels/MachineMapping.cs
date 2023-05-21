using AutoMapper;
using MoxControl.Connect.Models.Entities;

namespace MoxControl.ViewModels.MachineViewModels
{
    public class MachineMapping : Profile
    {
        public MachineMapping()
        {
            CreateMap<BaseMachine, MachineViewModel>();
            CreateMap<MachineViewModel, BaseMachine>();

            CreateMap<MachineCreateEditViewModel, BaseMachine>();
            CreateMap<BaseMachine, MachineCreateEditViewModel>();

            CreateMap<BaseMachine, MachineDetailsViewModel>()
                .ForMember(dest => dest.ServerId, opt => opt.MapFrom(src => src.Server.Id))
                .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Server.Name))
                .ForMember(dest => dest.VirtualizationSystem, opt => opt.MapFrom(src => src.Server.VirtualizationSystem))
                .ForMember(dest => dest.ServerHost, opt => opt.MapFrom(src => src.Server.Host))
                .ForMember(dest => dest.ServerPort, opt => opt.MapFrom(src => src.Server.Port));
            CreateMap<MachineDetailsViewModel, BaseMachine>();
        }
    }
}
