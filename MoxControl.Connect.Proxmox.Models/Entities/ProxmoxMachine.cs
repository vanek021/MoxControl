using MoxControl.Connect.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Models.Entities
{
    public class ProxmoxMachine : BaseMachine
    {
        [ForeignKey(nameof(ServerId))]
        public new ProxmoxServer Server { get; set; }
        public new long ServerId { get; set; }
    }
}
