using AutoMapper;
using MoxControl.Connect.Models.Entities;

namespace MoxControl.ViewModels.SettingViewModels
{
    public class SettingMapping : Profile
    {
        public SettingMapping()
        {
            CreateMap<ConnectSetting, ConnectSettingViewModel>();
            CreateMap<ConnectSettingViewModel, ConnectSetting>();
        }
    }
}
