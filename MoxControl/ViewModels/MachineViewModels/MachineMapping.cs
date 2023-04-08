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
                .ForMember(dest => dest.ServerId, opt => opt.Ignore())
                .ForMember(dest => dest.ServerName, opt => opt.Ignore())
                .ForMember(dest => dest.VirtualizationSystem, opt => opt.Ignore());
            CreateMap<MachineDetailsViewModel, BaseMachine>();
        }
    }
}
