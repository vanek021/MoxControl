using MoxControl.Connect.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.ServerViewModels
{
    public class ServerShortViewModel
    {
        public long Id { get; set; }
        public VirtualizationSystem VirtualizationSystem { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
