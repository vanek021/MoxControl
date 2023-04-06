using AutoMapper;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.ViewModels.MachineViewModels;
using MoxControl.ViewModels.ServerViewModels;

namespace MoxControl.Services
{
    public class MachineService
    {
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly IMapper _mapper;

        public MachineService(IConnectServiceFactory connectServiceFactory, IMapper mapper)
        {
            _connectServiceFactory = connectServiceFactory;
            _mapper = mapper;
        }

        public async Task<MachineIndexViewModel> GetMachineIndexViewModelAsync()
        {
            var machineIndexVm = new MachineIndexViewModel();

            var connectServices = _connectServiceFactory.GetAll();

            foreach (var connectService in connectServices)
            {
                var servers = await connectService.Item2.Servers.GetAllAsync();

                foreach (var server in servers)
                {
                    var machineListVm = new MachineListViewModel();
                    machineListVm.Server = _mapper.Map<ServerViewModel>(server);

                    var machines = await connectService.Item2.Machines.GetAllByServer(server.Id);
                    machineListVm.Machines = _mapper.Map<List<MachineViewModel>>(machines);

                    machineIndexVm.MachineLists.Add(machineListVm);
                }
            }

            return machineIndexVm;
        }


    }
}
