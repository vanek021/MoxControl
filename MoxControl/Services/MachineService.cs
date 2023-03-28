using AutoMapper;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.ViewModels.MachineViewModels;

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

            }

            return machineIndexVm;
        }
    }
}
