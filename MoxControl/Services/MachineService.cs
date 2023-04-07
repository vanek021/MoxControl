﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
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

        public async Task<MachineCreateEditViewModel> GetMachineViewModelForCreateAsync(VirtualizationSystem virtualizationSystem, long serverId)
        {
            var machineCreateEditVm = new MachineCreateEditViewModel
            {
                Templates = await GetTemplatesSelectListAsync(),
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

            return machineCreateEditVm;
        }

        public async Task<bool> CreateAsync(MachineCreateEditViewModel viewModel)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(viewModel.VirtualizationSystem);
            var machine = _mapper.Map<BaseMachine>(viewModel);
            
            var result = await connectService.Machines.CreateAsync(machine, viewModel.ServerId, viewModel.TemplateId);

            return result;
        }

        public async Task<bool> UpdateAsync(MachineCreateEditViewModel viewModel)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(viewModel.VirtualizationSystem);
            var machine = _mapper.Map<BaseMachine>(viewModel);

            var result = await connectService.Machines.UpdateAsync(machine);

            return result;
        }

        public async Task<SelectList> GetTemplatesSelectListAsync()
        {
            var templates = await _templateManager.GetAllAsync();
            var selectItems = templates.Select(x => new { Name = x.Name, Value = x.Id });
            return new SelectList(selectItems, "Value", "Name");
        }
    }
}
