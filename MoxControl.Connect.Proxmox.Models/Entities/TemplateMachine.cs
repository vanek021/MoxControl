using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Proxmox.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Models.Entities
{
    public class TemplateMachine : BaseMachine
    {
        [ForeignKey(nameof(ServerId))]
        public new ProxmoxServer Server { get; set; }
        public new long ServerId { get; set; }

        public string? ProxmoxName { get; set; }
        public int? ProxmoxId { get; set; }

        public TemplateMachineStatus Status { get; set; }
    }
}
