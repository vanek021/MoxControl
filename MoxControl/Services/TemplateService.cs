using MoxControl.Connect.Services;

namespace MoxControl.Services
{
    public class TemplateService
    {
        private readonly TemplateManager _templateManager;

        public TemplateService(TemplateManager templateManager)
        {
            _templateManager = templateManager;
        }
    }
}
