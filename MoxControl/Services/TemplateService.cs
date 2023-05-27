using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Services;
using MoxControl.ViewModels.TemplateViewModels;

namespace MoxControl.Services
{
    public class TemplateService
    {
        private readonly IMapper _mapper;
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly TemplateManager _templateManager;
        private readonly ImageManager _imageManager;

        public TemplateService(IMapper mapper, IConnectServiceFactory connectServiceFactory, TemplateManager templateManager, ImageManager imageManager)
        {
            _mapper = mapper;
            _connectServiceFactory = connectServiceFactory;
            _templateManager = templateManager;
            _imageManager = imageManager;
        }

        public async Task<TemplateIndexViewModel> GetTemplateIndexViewModelAsync()
        {
            var templates = await _templateManager.GetAllAsync();

            var templateIndexVm = new TemplateIndexViewModel
            {
                Templates = _mapper.Map<List<TemplateViewModel>>(templates)
            };

            return templateIndexVm;
        }

        public async Task<TemplateDetailsViewModel?> GetTemplateDetailsViewModelAsync(long templateId)
        {
            var template = await _templateManager.GetByIdAsync(templateId);

            if (template is null)
                return null;

            var templateVm = _mapper.Map<TemplateDetailsViewModel>(template);

            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();

                foreach (var server in servers)
                {
                    var serverVm = _mapper.Map<TemplateServerViewModel>(server);

                    if (server.TemplateData is not null && server.TemplateData.TemplateIds.Contains(template.Id))
                        serverVm.IsTemplateInitialized = true;

                    templateVm.Servers.Add(serverVm);
                }
            }

            return templateVm;
        }

        public async Task<TemplateCreateEditViewModel> GetTemplateViewModelForCreateAsync()
        {
            var templateVm = new TemplateCreateEditViewModel();

            templateVm.Images = await GetImagesSelectListAsync();

            return templateVm;
        }

        public async Task<TemplateCreateEditViewModel> GetTemplateViewModelForEditAsync(long templateId)
        {
            var template = await _templateManager.GetByIdAsync(templateId);
            var templateVm = _mapper.Map<TemplateCreateEditViewModel>(template);

            templateVm.Images = await GetImagesSelectListAsync();

            return templateVm;
        }

        public async Task<bool> CreateAsync(TemplateCreateEditViewModel viewModel)
        {
            var template = _mapper.Map<Template>(viewModel);

            var result = await _templateManager.CreateAsync(template);

            return result;
        }

        public async Task<bool> UpdateAsync(TemplateCreateEditViewModel viewModel)
        {
            var template = _mapper.Map<Template>(viewModel);

            var result = await _templateManager.UpdateAsync(template);

            return result;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var result = await _templateManager.DeleteAsync(id);
            return result;
        }

        public async Task<SelectList> GetImagesSelectListAsync()
        {
            var images = await _imageManager.GetAllAsync();
            var selectItems = images.Select(x => new { Name = x.Name, Value = x.Id });
            return new SelectList(selectItems, "Value", "Name");
        }
    }
}
