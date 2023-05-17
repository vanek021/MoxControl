using AutoMapper;
using MoxControl.Connect.Models.Entities;
using MoxControl.Models.Entities.Settings;

namespace MoxControl.ViewModels.SettingViewModels
{
    public class SettingMapping : Profile
    {
        public SettingMapping()
        {
            CreateMap<ConnectSetting, ConnectSettingViewModel>();
            CreateMap<ConnectSettingViewModel, ConnectSetting>();

            CreateMap<GeneralSetting, GeneralSettingViewModel>();
            CreateMap<GeneralSettingViewModel, GeneralSetting>();
        }
    }
}
