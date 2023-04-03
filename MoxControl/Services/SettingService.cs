using AutoMapper;
using MoxControl.Connect.Data;
using MoxControl.Connect.Models.Enums;
using MoxControl.ViewModels.SettingViewModels;

namespace MoxControl.Services
{
    public class SettingService
    {
        private readonly IMapper _mapper;
        private readonly ConnectDatabase _connectDatabase;

        public SettingService(IMapper mapper, ConnectDatabase connectDatabase)
        {
            _mapper = mapper;
            _connectDatabase = connectDatabase;
        }

        public async Task<ConnectSettingViewModel> GetConnectSettingViewModelAsync(VirtualizationSystem virtualizationSystem)
        {
            var connectSetting = await _connectDatabase.ConnectSettings.GetByVirtualizationSystemAsync(virtualizationSystem);

            var connectSettingVm = _mapper.Map<ConnectSettingViewModel>(connectSetting);

            return connectSettingVm;
        }

        public async Task<bool> UpdateConnectSettings(ConnectSettingViewModel viewModel)
        {
            var connectSetting = await _connectDatabase.ConnectSettings.GetByVirtualizationSystemAsync(viewModel.VirtualizationSystem);
            connectSetting = _mapper.Map(viewModel, connectSetting);

            try
            {
                _connectDatabase.ConnectSettings.Update(connectSetting);
                await _connectDatabase.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsVirtualizationSystemHasSettingsAsync(VirtualizationSystem virtualizationSystem)
        {
            var setting = await _connectDatabase.ConnectSettings.GetByVirtualizationSystemAsync(virtualizationSystem);
            return setting.IsShowSettingsSection;
        }
    }
}
