using MoxControl.Connect.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoxControl.Connect.Proxmox.Models.Entities
{
    public class ProxmoxMachine : BaseMachine
    {
        [ForeignKey(nameof(ServerId))]
        public new ProxmoxServer Server { get; set; }
        public new long ServerId { get; set; }

        public string? ProxmoxName { get; set; }
        public int? ProxmoxId { get; set; }
    }
}
