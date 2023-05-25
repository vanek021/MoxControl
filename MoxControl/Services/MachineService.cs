using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Models.Result;
using MoxControl.Connect.Services;
using MoxControl.Infrastructure.Extensions;
using MoxControl.ViewModels.MachineViewModels;
using MoxControl.ViewModels.ServerViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace MoxControl.Services
{
    public class MachineService
    {
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TemplateManager _templateManager;
        private readonly NotificationService _notificationService;

        public MachineService(IConnectServiceFactory connectServiceFactory, IHttpContextAccessor httpContextAccessor, 
            IMapper mapper, TemplateManager templateManager, NotificationService notificationService)
        {
            _connectServiceFactory = connectServiceFactory;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _templateManager = templateManager;
            _notificationService = notificationService;
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

                    var machines = await connectService.Item2.Machines.GetAllByServerAsync(server.Id);
                    machineListVm.Machines = _mapper.Map<List<MachineViewModel>>(machines);

                    machineIndexVm.MachineLists.Add(machineListVm);
                }
            }

            return machineIndexVm;
        }

        public async Task<MachineCreateEditViewModel> GetMachineViewModelForCreateAsync(VirtualizationSystem virtualizationSystem, long serverId)
        {
            var machineCreateEditVm = new MachineCreateEditViewModel
            {
                Templates = await GetTemplatesSelectListAsync(),
                Images = await GetImagesSelectListAsync(virtualizationSystem, serverId),
                VirtualizationSystem = virtualizationSystem,
                ServerId = serverId
            };

            return machineCreateEditVm;
        }

        public async Task<MachineCreateEditViewModel?> GetMachineViewModelForEditAsync(VirtualizationSystem virtualizationSystem, long machineId)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var machine = await connectService.Machines.GetAsync(machineId);

            if (machine is null)
                return null;

            var machineCreateEditVm = _mapper.Map<MachineCreateEditViewModel>(machine);

            machineCreateEditVm.Templates = await GetTemplatesSelectListAsync();
            machineCreateEditVm.Images = await GetImagesSelectListAsync(virtualizationSystem, machine.Server.Id);

            return machineCreateEditVm;
        }

        public async Task<bool> CreateAsync(MachineCreateEditViewModel viewModel)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(viewModel.VirtualizationSystem);
            var machine = _mapper.Map<BaseMachine>(viewModel);
            
            var result = await connectService.Machines.CreateAsync(machine, viewModel.ServerId, viewModel.TemplateId, viewModel.ImageId, _httpContextAccessor.HttpContext.GetUsername());

            return result;
        }

        public async Task<bool> UpdateAsync(MachineCreateEditViewModel viewModel)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(viewModel.VirtualizationSystem);
            var machine = _mapper.Map<BaseMachine>(viewModel);

            var result = await connectService.Machines.UpdateAsync(machine);

            return result;
        }

        public async Task<MachineDetailsViewModel?> GetMachineDetailsViewModelAsync(VirtualizationSystem virtualizationSystem, long machineId)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            await connectService.Machines.SendHeartBeatAsync(machineId, username);

            var machine = await connectService.Machines.GetAsync(machineId);

            if (machine is null)
                return null;

            var viewModel = _mapper.Map<MachineDetailsViewModel>(machine);
            viewModel.ConsoleHref = await connectService.Machines.GetConsoleSourceAsync(machineId);

            return viewModel;
        }

        public async Task<SelectList> GetTemplatesSelectListAsync()
        {
            var templates = await _templateManager.GetAllAsync();
            var selectItems = templates.Select(x => new { Name = x.Name, Value = x.Id });
            selectItems = selectItems.Prepend(new { Name = "Нет", Value = default(long) });
            return new SelectList(selectItems, "Value", "Name");
        }

        public async Task<SelectList> GetImagesSelectListAsync(VirtualizationSystem virtualizationSystem, long serverId)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            var availableImages = await connectService.Servers.GetAvailableImagesAsync(serverId);
            var selectItems = availableImages.Select(x => new { Name = x.Name, Value = x.Id });
            selectItems = selectItems.Prepend(new { Name = "Нет", Value = default(long) });
            return new SelectList(selectItems, "Value", "Name");
        }

        public async Task<MachineHealthModel?> GetMachineHealthModelAsync(VirtualizationSystem virtualizationSystem, long machineId)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var healthModel = await connectService.Machines.GetHealthModelAsync(machineId, _httpContextAccessor?.HttpContext?.User?.Identity?.Name);

            return healthModel;
        }

        public async Task<bool> TurnOnMachine(VirtualizationSystem virtualizationSystem, long machineId)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var result = await connectService.Machines.TurnOnAsync(machineId);

            if (!result.Success)
                await WriteErrorNotification(result);

            return result.Success;
        }

        public async Task<bool> TurnOffMachine(VirtualizationSystem virtualizationSystem, long machineId)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var result = await connectService.Machines.TurnOffAsync(machineId);

            if (!result.Success)
                await WriteErrorNotification(result);

            return result.Success;
        }

        public async Task<bool> RebootMachine(VirtualizationSystem virtualizationSystem, long machineId)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var result = await connectService.Machines.RebootAsync(machineId);

            if (!result.Success)
                await WriteErrorNotification(result);

            return result.Success;
        }

        public async Task<bool> HardRebootMachine(VirtualizationSystem virtualizationSystem, long machineId)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            var result = await connectService.Machines.HardRebootAsync(machineId);

            if (!result.Success)
                await WriteErrorNotification(result);

            return result.Success;
        }

        private async Task WriteErrorNotification(BaseResult result)
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            await _notificationService.AddErrorAsync(username ?? "", "Ошибка при перезагрузке ВМ", result.ErrorMessage!);
        }
    }
}
