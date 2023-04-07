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
        }
    }
}
