using AutoMapper;
using MoxControl.Connect.Data;
using MoxControl.Connect.Models.Enums;
using MoxControl.Data;
using MoxControl.ViewModels.SettingViewModels;

namespace MoxControl.Services
{
    public class SettingService
    {
        private readonly IMapper _mapper;
        private readonly ConnectDatabase _connectDatabase;
        private readonly Database _database;

        public SettingService(IMapper mapper, ConnectDatabase connectDatabase, Database database)
        {
            _mapper = mapper;
            _connectDatabase = connectDatabase;
            _database = database;
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

        public async Task<SettingIndexViewModel> GetSettingIndexViewModelAsync()
        {
            var settings = await _database.GeneralSettings.GetAll();

            var settingIndexVm = new SettingIndexViewModel
            {
                Settings = _mapper.Map<List<GeneralSettingViewModel>>(settings)
            };

            return settingIndexVm;
        }

        public async Task<GeneralSettingViewModel> GetGeneralSettingViewModelAsync(long id)
        {
            var setting = await _database.GeneralSettings.GetByIdAsync(id);

            return _mapper.Map<GeneralSettingViewModel>(setting);
        }

        public async Task<bool> UpdateGeneralSetting(GeneralSettingViewModel viewModel)
        {
            var setting = await _database.GeneralSettings.GetByIdAsync(viewModel.Id);
            setting.Value = viewModel.Value;

            try
            {
                _database.GeneralSettings.Update(setting);
                await _database.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
