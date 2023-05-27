using AutoMapper;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Enums;
using MoxControl.ViewModels.MachineViewModels;
using MoxControl.ViewModels.ServerViewModels;

namespace MoxControl.Services
{
    public class ServerService
    {
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ServerService(IConnectServiceFactory connectServiceFactory, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectServiceFactory = connectServiceFactory;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(ServerViewModel viewModel)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(viewModel.VirtualizationSystem);

            var result = await connectService.Servers.CreateAsync(viewModel.Host, viewModel.Port, viewModel.AuthorizationType, viewModel.Name,
                viewModel.Description, viewModel.RootLogin, viewModel.RootPassword, _httpContextAccessor?.HttpContext?.User?.Identity?.Name);

            return result;
        }

        public async Task<bool> UpdateAsync(ServerViewModel viewModel)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(viewModel.VirtualizationSystem);

            var result = await connectService.Servers.UpdateAsync(viewModel.Id, viewModel.Host, viewModel.Port, viewModel.AuthorizationType,
                viewModel.Name, viewModel.Description, viewModel.RootLogin, viewModel.RootPassword, _httpContextAccessor?.HttpContext?.User?.Identity?.Name);

            return result;
        }

        public async Task<bool> DeleteAsync(VirtualizationSystem virtualizationSystem, long id)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var result = await connectService.Servers.DeleteAsync(id);

            return result;
        }

        public async Task<ServerViewModel> GetServerViewModelAsync(long id, VirtualizationSystem virtualizationSystem)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var server = await connectService.Servers.GetAsync(id);
            var serverVm = _mapper.Map<ServerViewModel>(server);

            return serverVm;
        }

        public async Task<ServerIndexViewModel> GetServerIndexViewModelAsync()
        {
            var serverIndexVm = new ServerIndexViewModel();

            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();
                var serversVm = _mapper.Map<List<ServerViewModel>>(servers);

                serverIndexVm.ServerLists.Add(new ServerListViewModel()
                {
                    VirtualizationSystem = connectServiceItem.VirtualizationSystem,
                    Servers = serversVm
                });
            }

            return serverIndexVm;
        }

        public async Task<ServerDetailsViewModel> GetServerDetailsViewModelAsync(VirtualizationSystem virtualizationSystem, long id)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var server = await connectService.Servers.GetAsync(id);
            var machines = await connectService.Machines.GetAllByServerAsync(id);

            var serverDetailsVm = _mapper.Map<ServerDetailsViewModel>(server);
            serverDetailsVm.Machines = _mapper.Map<List<MachineViewModel>>(machines);

            return serverDetailsVm;
        }

        public async Task<ServerHealthModel?> GetServerHealthModelAsync(VirtualizationSystem virtualizationSystem, long serverId)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var healthModel = await connectService.Servers.GetHealthModelAsync(serverId, _httpContextAccessor?.HttpContext?.User?.Identity?.Name);

            return healthModel;
        }
    }
}
