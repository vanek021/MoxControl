using AutoMapper;
using MoxControl.Connect.Models.Entities;

namespace MoxControl.ViewModels.TemplateViewModels
{
    public class TemplateMapping : Profile
    {
        public TemplateMapping()
        {
            CreateMap<MachineTemplate, TemplateViewModel>();
            CreateMap<TemplateViewModel, MachineTemplate>();
        }
    }
}
