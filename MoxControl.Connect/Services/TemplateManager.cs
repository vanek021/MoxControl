using Hangfire;
using Microsoft.AspNetCore.Http;
using MoxControl.Connect.Data;
using MoxControl.Connect.Models.Entities;
using MoxControl.Infrastructure.Extensions;

namespace MoxControl.Connect.Services
{
    public class TemplateManager
    {
        private readonly ConnectDatabase _connectDatabase;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TemplateManager(ConnectDatabase connectDatabase, IHttpContextAccessor httpContextAccessor)
        {
            _connectDatabase = connectDatabase;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> GetTotalCount()
        {
            return await _connectDatabase.Templates.GetTotalCount();
        }

        public async Task<List<Template>> GetAllAsync()
        {
            return await _connectDatabase.Templates.GetAllAsync();
        }

        public async Task<Template?> GetByIdAsync(long id)
        {
            return await _connectDatabase.Templates.GetByIdAsync(id);
        }

        public async Task<Template?> GetByIdWithImageAsync(long id)
        {
            return await _connectDatabase.Templates.GetByIdWithImageAsync(id);
        }

        public async Task<bool> CreateAsync(Template template)
        {
            _connectDatabase.Templates.Insert(template);

            try
            {
                await _connectDatabase.SaveChangesAsync();
                BackgroundJob.Enqueue<HangfireConnectManager>(h => h.HandleTemplateCreateForAllServersAsync(template.Id, _httpContextAccessor.HttpContext.GetUsername()));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Template template)
        {
            _connectDatabase.Templates.Update(template);

            try
            {
                await _connectDatabase.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var template = await _connectDatabase.Templates.GetByIdAsync(id);

            if (template is null)
                return false;

            _connectDatabase.Templates.Delete(template);

            try
            {
                await _connectDatabase.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task MarkAsReadyToUseAsync(long templateId)
        {
            var template = await _connectDatabase.Templates.GetByIdAsync(templateId);

            if (template is null)
                return;

            template.Status = Models.Enums.TemplateStatus.ReadyToUse;
            await _connectDatabase.SaveChangesAsync();
        }
    }
}
