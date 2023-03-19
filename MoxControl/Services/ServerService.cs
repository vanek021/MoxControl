using AutoMapper;
using MoxControl.Connect.Factory;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox.Models;
using MoxControl.Data;
using MoxControl.Services.Abtractions;
using MoxControl.Services.Models;
using MoxControl.ViewModels.ServerViewModels;
using System.Drawing;

namespace MoxControl.Services
{
    public class ServerService
    {
        private readonly ConnectServiceFactory _connectServiceFactory;
        private readonly IMapper _mapper;

        public ServerService(ConnectServiceFactory connectServiceFactory, IMapper mapper)
        {
            _connectServiceFactory = connectServiceFactory;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(ServerViewModel viewModel)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(viewModel.VirtualizationSystem);

            return await connectService.CreateServerAsync(viewModel.Host, viewModel.Port, viewModel.AuthorizationType, viewModel.Name, 
                viewModel.Description, viewModel.RootLogin, viewModel.RootPassword);
        }

        public async Task<bool> UpdateAsync(ServerViewModel viewModel)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(viewModel.VirtualizationSystem);

            return await connectService.UpdateServerAsync(viewModel.Id, viewModel.Host, viewModel.Port, viewModel.AuthorizationType,
                viewModel.Name, viewModel.Description, viewModel.RootLogin, viewModel.RootPassword);
        }

        public async Task<ServerViewModel> GetServerViewModel(long id, VirtualizationSystem virtualizationSystem)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var server = await connectService.GetServerAsync(id);
            var serverVm = _mapper.Map<ServerViewModel>(server);

            return serverVm;
        }
    }
}
