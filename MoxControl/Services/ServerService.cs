using AutoMapper;
using MoxControl.Connect.Enums;
using MoxControl.Connect.Models;
using MoxControl.Connect.Proxmox.Models;
using MoxControl.Data;
using MoxControl.Services.Abtractions;
using MoxControl.Services.Models;
using MoxControl.ViewModels.ServerViewModels;
using System.Drawing;

namespace MoxControl.Services
{
    public class ServerService : ServiceBase<BaseServer>
    {
        private readonly IMapper _mapper;

        public ServerService(IMapper mapper, AppDbContext context) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(ServerViewModel viewModel)
        {
            return viewModel.VirtualizationSystem switch
            {
                VirtualizationSystem.Proxmox => await CreateAsync<ProxmoxServer>(viewModel),
                _ => throw new NotImplementedException("Система виртуализации не реализована"),
            };
        }

        public async Task<bool> UpdateAsync(ServerViewModel viewModel)
        {
            switch (viewModel.VirtualizationSystem)
            {
                case VirtualizationSystem.Proxmox:
                    var dbModel = DbContext.ProxmoxServers.First(x => x.Id == viewModel.Id);
                    return await UpdateAsync<ProxmoxServer>(viewModel, dbModel);
                default:
                    throw new NotImplementedException("Система виртуализации не реализована");
            }
        }

        public ServerViewModel? GetServerViewModel(long id, VirtualizationSystem virtualizationSystem)
        {
            switch (virtualizationSystem)
            {
                case VirtualizationSystem.Proxmox:
                    var dbModel = DbContext.ProxmoxServers.FirstOrDefault(x => x.Id == id);
                    var viewModel = _mapper.Map<ProxmoxServer, ServerViewModel>(dbModel);
                    return viewModel;
                default:
                    throw new NotImplementedException();
            }
        }

        //public ServerIndexViewModel GetServerIndexViewModel()
        //{
        //    var viewModel = new ServerIndexViewModel();

        //    foreach (int i in Enum.GetValues(typeof(VirtualizationSystem)))
        //    {
        //        Console.WriteLine($" {i}");
        //    }
        //}

        private async Task<bool> CreateAsync<T>(ServerViewModel viewModel) where T : class
        {
            var model = _mapper.Map<ServerViewModel, T>(viewModel);

            DbContext.Add<T>(model);

            try
            {
                await SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }           
        }

        private async Task<bool> UpdateAsync<T>(ServerViewModel viewModel, T dbModel) where T : class
        {
            dbModel = _mapper.Map(viewModel, dbModel);

            DbContext.Update<T>(dbModel);

            try
            {
                await SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
