using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.ViewModels
{
    public class ProxmoxServerSettingViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Имя базового узла")]
        public string BaseNode { get; set; }
    }
}
