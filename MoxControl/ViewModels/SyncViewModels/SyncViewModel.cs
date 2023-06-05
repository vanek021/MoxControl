namespace MoxControl.ViewModels.SyncViewModels
{
    public class SyncViewModel
    {
        public List<SyncServerViewModel> SyncServers { get; set; } = new();
        public SyncTemplateViewModel SyncTemplate { get; set; } = new();
        public SyncImageViewModel SyncImage { get; set; } = new();
    }
}
