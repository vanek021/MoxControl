using AutoMapper;
using Hangfire;
using MoxControl.Connect.Factory;
using MoxControl.Connect.Interfaces;
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
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly IMapper _mapper;

        public ServerService(IConnectServiceFactory connectServiceFactory, IMapper mapper)
        {
            _connectServiceFactory = connectServiceFactory;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(ServerViewModel viewModel)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(viewModel.VirtualizationSystem);

            var result = await connectService.CreateServerAsync(viewModel.Host, viewModel.Port, viewModel.AuthorizationType, viewModel.Name, 
                viewModel.Description, viewModel.RootLogin, viewModel.RootPassword);
            
            return result;
        }

        public async Task<bool> UpdateAsync(ServerViewModel viewModel)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(viewModel.VirtualizationSystem);

            var result = await connectService.UpdateServerAsync(viewModel.Id, viewModel.Host, viewModel.Port, viewModel.AuthorizationType,
                viewModel.Name, viewModel.Description, viewModel.RootLogin, viewModel.RootPassword);

            return result;
        }

        public async Task<ServerViewModel> GetServerViewModelAsync(long id, VirtualizationSystem virtualizationSystem)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var server = await connectService.GetServerAsync(id);
            var serverVm = _mapper.Map<ServerViewModel>(server);

            return serverVm;
        }

        public async Task<ServerIndexViewModel> GetServerIndexViewModelAsync()
        {
            var serverIndexVm = new ServerIndexViewModel();

            var connectServices = _connectServiceFactory.GetAll();

            foreach(var connectService in connectServices)
            {
                var servers = await connectService.Item2.GetAllServersAsync();
                var serversVm = _mapper.Map<List<ServerViewModel>>(servers);

                serverIndexVm.ServerLists.Add(new ServerListViewModel()
                {
                    VirtualizationSystem = connectService.Item1,
                    Servers = serversVm
                });
            }

            return serverIndexVm;
        }
    }
}
