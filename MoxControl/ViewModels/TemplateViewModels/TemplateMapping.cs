using AutoMapper;
using MoxControl.Connect.Models.Entities;

namespace MoxControl.ViewModels.TemplateViewModels
{
    public class TemplateMapping : Profile
    {
        public TemplateMapping()
        {
            CreateMap<Template, TemplateViewModel>();
            CreateMap<TemplateViewModel, Template>();

            CreateMap<Template, TemplateCreateEditViewModel>();
            CreateMap<TemplateCreateEditViewModel, Template>();

            CreateMap<Template, TemplateDetailsViewModel>();
            CreateMap<TemplateDetailsViewModel, Template>();
        }
    }
}
