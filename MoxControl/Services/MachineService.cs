using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Services;
using MoxControl.ViewModels.MachineViewModels;
using MoxControl.ViewModels.ServerViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace MoxControl.Services
{
    public class MachineService
    {
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly IMapper _mapper;
        private readonly TemplateManager _templateManager;

        public MachineService(IConnectServiceFactory connectServiceFactory, IMapper mapper, TemplateManager templateManager)
        {
            _connectServiceFactory = connectServiceFactory;
            _mapper = mapper;
            _templateManager = templateManager;
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
                    var machineListVm = new MachineListViewModel
                    {
                        Server = _mapper.Map<ServerViewModel>(server)
                    };

                    var machines = await connectService.Item2.Machines.GetAllByServer(server.Id);
                    machineListVm.Machines = _mapper.Map<List<MachineViewModel>>(machines);

                    machineIndexVm.MachineLists.Add(machineListVm);
                }
            }

            return machineIndexVm;
        }

        public async Task<MachineCreateEditViewModel> GetMachineViewModelForCreateAsync()
        {
            var machineCreateEditVm = new MachineCreateEditViewModel
            {
                Templates = await GetTemplatesSelectListAsync()
            };

            return machineCreateEditVm;
        }

        public async Task<SelectList> GetTemplatesSelectListAsync()
        {
            var templates = await _templateManager.GetAllAsync();
            var selectItems = templates.Select(x => new { Name = x.Name, Value = x.Id });
            return new SelectList(selectItems, "Value", "Name");
        }
    }
}
