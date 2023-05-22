using Hangfire;
using MoxControl.Connect.Data;
using MoxControl.Connect.Models.Entities;

namespace MoxControl.Connect.Services
{
    public class TemplateManager
    {
        private readonly ConnectDatabase _connectDatabase;

        public TemplateManager(ConnectDatabase connectDatabase)
        {
            _connectDatabase = connectDatabase;
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
    }
}
